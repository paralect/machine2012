using System;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Paralect.Machine.Mongo
{
    public class MongoJournalServer
    {
        /// <summary>
        /// MongoDB Server
        /// </summary>
        private readonly MongoServer _server;

        /// <summary>
        /// Name of database 
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// Collections for storing messages and head info
        /// </summary>
        private const string _journalMessagesCollectionName = "journal.messages";
        private const string _journalHeadCollectionName = "journal.head";

        private readonly MongoCollectionSettings<BsonDocument> _journalMessagesSettings;
        private readonly MongoCollectionSettings<BsonDocument> _journalHeadSettings;

        /// <summary>
        /// Opens connection to MongoDB Server
        /// </summary>
        public MongoJournalServer(String connectionString)
        {
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _server = MongoServer.Create(connectionString);

            _journalMessagesSettings = Database.CreateCollectionSettings<BsonDocument>(_journalMessagesCollectionName);
            _journalMessagesSettings.SafeMode = SafeMode.True;
            _journalMessagesSettings.AssignIdOnInsert = false;

            _journalHeadSettings = Database.CreateCollectionSettings<BsonDocument>(_journalHeadCollectionName);
            _journalHeadSettings.SafeMode = SafeMode.True;
            _journalHeadSettings.AssignIdOnInsert = false;
        }

        /// <summary>
        /// MongoDB Server
        /// </summary>
        public MongoServer Server
        {
            get { return _server; }
        }

        /// <summary>
        /// Get database
        /// </summary>
        public MongoDatabase Database
        {
            get { return _server.GetDatabase(_databaseName); }
        }

        /// <summary>
        /// Get messages collection
        /// </summary>
        public MongoCollection<BsonDocument> Messages
        {
            get { return Database.GetCollection(_journalMessagesCollectionName); }
        }

        /// <summary>
        /// Get heads collection
        /// </summary>
        public MongoCollection<BsonDocument> Heads
        {
            get { return Database.GetCollection(_journalHeadCollectionName); }
        }

        public Int64 GetCurrentSequence()
        {
            var doc = Heads
                .FindOneAs<BsonDocument>(Query.EQ("_id", "head"));

            if (doc == null)
                return 0;

            return doc["Seq"].ToInt64();
        }

        public void SaveCurrentSequence(Int64 seq)
        {
            Heads.Update(
                Query.EQ("_id", "head"), Update.Set("Seq", seq),
                UpdateFlags.Upsert, SafeMode.True);
        }
    }
}