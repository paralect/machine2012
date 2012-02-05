using System;
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
        public Int64 Save(IEnumerable<IMessageEnvelope> binaryMessageEnvelopes)
        {
            // TODO: We should use here 2PC in order to update seq and message collection

            var seq = _server.GetCurrentSequence();
            seq++; // next available sequence

            var list = new List<BsonDocument>();

            foreach (var binaryMessageEnvelope in binaryMessageEnvelopes)
            {
/*                var doc = new BsonDocument();
                SetHeaderInfo(doc, binaryMessageEnvelope.GetHeader());

                doc["Header"] = new BsonBinaryData(binaryMessageEnvelope.Header);
                doc["Message"] = new BsonBinaryData(binaryMessageEnvelope.Message);
                doc["Seq"] = seq++;
                list.Add(doc);
 * 
 */
            }

            var result = _server.Messages.InsertBatch(list, SafeMode.True);

            seq--; // current seq number

            _server.SaveCurrentSequence(seq);

            return seq;
        }

        public void SetHeaderInfo(BsonDocument doc, Header header)
        {
            var fields = header.GetFields();

            foreach (var field in fields)
            {
                doc[field.Key] = BsonValue.Create(field.Value);
            }
        }
    }
}