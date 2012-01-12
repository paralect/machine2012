﻿using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace Paralect.Machine.Mongo.Tests.Helpers.Mongo
{
    public class MongoSerializer
    {
        public static Object Deserialize(BsonDocument doc, Type type)
        {
            return BsonSerializer.Deserialize(doc, type);
        }

        public static BsonDocument Serialize(Object obj)
        {
            BsonDocument data = new BsonDocument();
            var writer = BsonWriter.Create(data);
            BsonSerializer.Serialize(writer, obj.GetType(), obj);
            return data;
        }
    }
}