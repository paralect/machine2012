﻿using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Paralect.Machine.Journals.Abstract;
using Paralect.Machine.Messages;

namespace Paralect.Machine.Mongo.Journals
{
    public class MongoJournalStorage : IJournalStorage
    {
        private readonly MongoJournalServer _server;

        public MongoJournalStorage(MongoJournalServer server)
        {
            _server = server;
        }

        /// <summary>
        /// Returns HEAD of sequence number
        /// </summary>
        public Int64 Save(IEnumerable<IPacketMessageEnvelope> messageEnvelopes)
        {
            // TODO: We should use here 2PC in order to update seq and message collection

            var seq = _server.GetCurrentSequence();
            seq++; // next available sequence

            var list = new List<BsonDocument>();

            foreach (var messageEnvelope in messageEnvelopes)
            {
                var doc = new BsonDocument();
                SetHeaderInfo(doc, messageEnvelope.Metadata);

                doc["Header"] = new BsonBinaryData(messageEnvelope.MetadataBinary);
                doc["Message"] = new BsonBinaryData(messageEnvelope.MessageBinary);
                doc["Seq"] = seq++;
                list.Add(doc);
            }

            _server.SaveCurrentSequence(seq);

            var result = _server.Messages.InsertBatch(list, SafeMode.True);

            seq--; // current seq number

            

            return seq;
        }

        public IList<IPacketMessageEnvelope> Load(long greatorOrEqualThan, int count)
        {
            throw new NotImplementedException();
        }

        public void SetHeaderInfo(BsonDocument doc, IMessageMetadata metadata)
        {
            doc["MessageTag"] = metadata.MessageTag;
        }
    }
}