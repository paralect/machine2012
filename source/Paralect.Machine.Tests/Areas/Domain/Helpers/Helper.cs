using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paralect.Machine.Domain;
using Paralect.Machine.Transitions;

namespace Paralect.Domain.Test
{
    public class Helper
    {
        public static Repository GetRepository()
        {
            return new Repository(GetTransitionStorage(), null, new AssemblyQualifiedDataTypeRegistry());
        }

        public static ITransitionStorage GetTransitionStorage()
        {
            //var server = new TransitionStorage(GetTransitionRepository());
            return null;
//            return server;
        }

/*
        public static MongoTransitionRepository GetTransitionRepository()
        {
            return new MongoTransitionRepository(
                new AssemblyQualifiedDataTypeRegistry(),
                GetConnectionString());
        }
*/

        public static String GetConnectionString()
        {
            return "mongodb://admin(admin):1@localhost:27020/test";
        }
    }
}
