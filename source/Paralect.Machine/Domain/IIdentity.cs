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
    /// 
    /// Memory structure:
    /// 
    /// |------------------|--------------------|-------------------|
    /// |  Tag (3 bytes)   |  Version (1 byte)  |  ID (unlimited)   |
    /// |------------------|--------------------|-------------------|
    /// 
    /// Tag is number from 0 to 16.777.215.
    /// Version is a number fr
    /// 
    /// </summary>
    public interface IIdentity
    {
/*        Byte[] Data { get; }

        Int32 Tag { get; }
        Int32 Version { get; }*/

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
        private Byte[] _data;

        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// Gets the id, converted to a string. Only alphanumerics and '-' are allowed.
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            /*
            byte[] b = { 1, 2, 3 };
            //int m = (*b) | b

//            MemoryStream stream = new MemoryStream(_bindata);
            StreamReader reader = new StreamReader(stream);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write();*/
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
