using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Paralect.Machine.Domain
{
    /// <summary>
    /// Strongly-typed identity class. Essentially just an ID with a 
    /// distinct type. It introduces strong-typing and speeds up development
    /// on larger projects. Idea by Jeremie, implementation approach stolen from Rinat
    /// TODO: I think identity internally should be binary represented. 
    /// TODO: It will be better for memory and still possible to use binary as ID for various DBs (like BinData in MongoDB)
    /// </summary>
    public interface IIdentity
    {
//        Byte[] ByteArray { get; }

  //      String ToBase64String();



        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        /// <returns></returns>
//        String GetId();

//        String ToBase54String();

        /// <summary>
        /// Unique tag (should be unique within the assembly) to distinguish
        /// between different identities, while deserializing.
        /// </summary>
     //   String EntityTag { get; }
    }

    
    public abstract class AbstractIdentity : IIdentity
    {
        private Byte[] _bindata;

        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            MemoryStream stream = new MemoryStream(_bindata);
            StreamReader reader = new StreamReader(stream);
            return null;
        }

        /// <summary>
        /// Unique tag (should be unique within the assembly) to distinguish
        /// between different identities, while deserializing.
        /// </summary>
        public string EntityTag
        {
            get { throw new NotImplementedException(); }
        }

//        public void 
    }
}
