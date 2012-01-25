using System;
using System.Diagnostics;
using MongoDB.Bson;
using NUnit.Framework;
using Paralect.Machine.Tests.Helpers.Mongo;
using Paralect.Machine.Tests.Helpers.Protobuf;
using ProtoBuf;

namespace Paralect.Machine.Tests.Areas.Performance.Fixtures
{
    [TestFixture]
    public class SerializationPerformanceTest
    {
        private const int _iterations = 10000;

        [Ignore]
        public void bson_serialize_and_deserialise_performance()
        {
            var obj = Developer.Create();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var bson = MongoSerializer.Serialize(obj);
                var back = (Developer) MongoSerializer.Deserialize(bson, typeof(Developer));
            }
            watch.Stop();

            Console.WriteLine("Bson. Done in {0} msec", watch.ElapsedMilliseconds);            
        }

        [Ignore]
        public void protobuf_serialize_and_deserialise_performance()
        {
            var obj = Developer.Create();

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < _iterations; i++)
            {
                var bytes = ProtobufSerializer.SerializeProtocalBuffer(obj);
                var back = ProtobufSerializer.DeserializeProtocalBuffer<Developer>(bytes);
            }
            watch.Stop();

            Console.WriteLine("Protobuf. Done in {0} msec", watch.ElapsedMilliseconds);
        }

        [Ignore]
        public void protobuf_in_bson()
        {
            var obj = Developer.Create();
            var bytes = ProtobufSerializer.SerializeProtocalBuffer(obj);

            var doc = new BsonDocument()
            {
                { "Hello", "bye" },
                { "ProtoBuf", bytes }
            };

            Assert.That(doc["Hello"].AsString, Is.EqualTo("bye"));
        }

    }

    #region Helper classes

    [ProtoContract]
    public class Developer
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
        
        [ProtoMember(3)]
        public string ProgrammingLanguage { get; set; }

        [ProtoMember(4)]
        public string FavoriteColor { get; set; }

        [ProtoMember(5)]
        public string StudentId { get; set; }

        [ProtoMember(6)]
        public string SchoolId { get; set; }

        [ProtoMember(7)]
        public string SecretKey { get; set; }

        [ProtoMember(8)]
        public Int32 Status { get; set; }

        [ProtoMember(9)]
        public bool ParentSheduledLetterWereGenerated { get; set; }

        [ProtoMember(10)]
        public string OffenseId { get; set; }

        [ProtoMember(11)]
        public string OffenseCodeDefault { get; set; }

        [ProtoMember(12)]
        public string OffenseCodeCustom { get; set; }

        [ProtoMember(13)]
        public string OffenseName { get; set; }

        [ProtoMember(14)]
        public DateTime? OffenseDateTime { get; set; }

        [ProtoMember(15)]
        public int? OffenseTimeInMinutes { get; set; }

        [ProtoMember(16)]
        public string OffenseLocation { get; set; }

        [ProtoMember(17)]
        public string OffenseNote { get; set; }

        [ProtoMember(18)]
        public DateTime? CreateDate { get; set; }

        [ProtoMember(19)]
        public DateTime? Completed { get; set; }

        [ProtoMember(20)]
        public DateTime? ScheduleDate { get; set; }

        [ProtoMember(21)]
        public string ScheduledTime { get; set; }

        [ProtoMember(22)]
        public String LocationId { get; set; }

        [ProtoMember(23)]
        public string Location { get; set; }

        [ProtoMember(24)]
        public DateTime? SentDate { get; set; }

        [ProtoMember(25)]
        public string SentBy { get; set; }

        [ProtoMember(26)]
        public string SentById { get; set; }

        [ProtoMember(27)]
        public bool Sent { get; set; }

        [ProtoMember(28)]
        public bool IsDemo { get; set; }

        [ProtoMember(29)]
        public string ModuleSequence { get; set; }

        public static Developer Create()
        {
            var d = new Developer()
            {
                Name = "Helloasdfasdf",
                Id = 4544,
                SchoolId = "asafadsfasdfasdfa",
                FavoriteColor = "afgagsfgsfd",
                ProgrammingLanguage = "asdfasdfadsf",
                Completed = DateTime.Now,
                CreateDate = DateTime.Now,
                IsDemo = true,
                Location = "asdfasdf",
                LocationId = "32542345245452",
                ModuleSequence = "45453433",
                OffenseCodeCustom = "12341234",
                OffenseCodeDefault = "asdfadfasdfadasdf",
                OffenseDateTime = DateTime.Now,
                OffenseId = "adsfasdadsfasdf",
                OffenseLocation = "asdfadfsfgsdfsfg",
                OffenseName = "asdfadfadfadf",
                OffenseNote = "wrtwrtwrtwtwrt",
                OffenseTimeInMinutes = 5656,
                ParentSheduledLetterWereGenerated = true,
                ScheduleDate = DateTime.Now,
                ScheduledTime = "asdfasdfadf",
                SecretKey = "ajksdfkahsdfl",
                Sent = true,
                SentBy = "Hello",
                SentById = "afsdfasdfasdf",
                SentDate = DateTime.Now,
                Status = 245254,
                StudentId = "adfwrtwierutwirtqwewr",
            };

            return d;
        }
    }

    #endregion
}