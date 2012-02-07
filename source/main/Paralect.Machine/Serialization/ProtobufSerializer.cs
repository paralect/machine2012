using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using Paralect.Machine.Identities;
using Paralect.Machine.Messages;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Paralect.Machine.Serialization
{
    /// <summary>
    /// This protobuf serializer reserves tag numbers from 10,000,000 to 26,777,215 inclusively. 
    /// If your message has tag numbers from this range - behaviour of the serializer is undefined.
    /// </summary>
    public class ProtobufSerializer
    {
        /// <summary>
        /// Protobuf .net model of types that should be serialized and deserialized
        /// </summary>
        private readonly RuntimeTypeModel _model = TypeModel.Create();

        /// <summary>
        /// Base type --> ( Proto tag --> Child type )
        /// </summary>
        private readonly ConcurrentDictionary<Type, Dictionary<Int32, Type>> _map = new ConcurrentDictionary<Type, Dictionary<int, Type>>();

        /// <summary>
        /// Offset for automatic hierarchy registration
        /// </summary>
        private const int _tagOffset = 10000000;

        /// <summary>
        /// Protobuf .net model of types that should be serialized and deserialized
        /// </summary>
        public RuntimeTypeModel Model
        {
            get { return _model; }
        }

        /// <summary>
        /// Register message definitions
        /// </summary>
        public void RegisterMessages(IEnumerable<MessageDefinition> definitions)
        {
            foreach (var definition in definitions)
                RegisterType(definition.Type, definition.Tag);
        }

        /// <summary>
        /// Register message definitions
        /// </summary>
        public void RegisterIdentities(IEnumerable<IdentityDefinition> definitions)
        {
            foreach (var definition in definitions)
                RegisterType(definition.Type, definition.Tag);
        }

        /// <summary>
        /// TODO: Implement this (when you'll start to implement snapshotting)
        /// </summary>
        public void RegisterProcessStates()
        {
            throw new NotImplementedException("Snapshotting not available yet.");
        }

        /// <summary>
        /// General registration of types in order to support semi-automatic serailization of hierarchies of objects (but not interfaces!)
        /// </summary>
        private void RegisterType(Type type, Guid typeTag)
        {
            var baseType = type.BaseType;
            var tag = GenerateHierarchyTag(typeTag);

            if (baseType != typeof (object) && baseType != null)
            {
                if (!_map.ContainsKey(baseType))
                    _map[baseType] = new Dictionary<int, Type>();

                var tagToChild = _map[baseType];

                if (tagToChild.ContainsKey(tag))
                    throw new ProtoHierarchyTagCollision(tag, tagToChild[tag], type);

                tagToChild[tag] = type;

                _model[baseType]
                    .AddSubType(tag + _tagOffset, type);
            }
        }

        public byte[] Serialize(Object obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _model.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public TObject Deserialize<TObject>(byte[] data)
        {
            return (TObject) Deserialize(data, typeof(TObject));
        }

        public Object Deserialize(byte[] data, Type objType)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                Object obj = _model.Deserialize(memoryStream, null, objType);
                return obj;
            }
        }

        public TObject SerializeAndDeserialize<TObject>(Object obj)
        {
            var bytes = Serialize(obj);
            return Deserialize<TObject>(bytes);
        }

        public Object SerializeAndDeserialize(Object obj, Type objType)
        {
            var bytes = Serialize(obj);
            return Deserialize(bytes, objType);
        }

        /// <summary>
        /// Returns Int32 value from 0 to 16,777,215.
        /// </summary>
        public static Int32 GenerateHierarchyTag(Guid guid)
        {
            var hashcode = guid.GetHashCode();

            // Make hashcode positive
            hashcode = Math.Abs(hashcode);

            // Take 3 lowest bytes. It is from 0 to 16,777,215
            const int mask = 0x00ffffff;
            var tag = hashcode & mask;

            return tag;
        }
    }

    /*
    public struct AlreadyRegisteredInHierarchyTypes
    {
        public Type BaseType { get; set; }
        public Type ChildType { get; set; }

        public AlreadyRegisteredInHierarchyTypes(Type baseType, Type childType)
            : this()
        {
            BaseType = baseType;
            ChildType = childType;
        }

        public override int GetHashCode()
        {
            // More about this here: http://stackoverflow.com/a/720282/407599
            //             and here: http://www.pcreview.co.uk/forums/writing-own-gethashcode-function-t3182933.html

            int hash = 27;
            hash = (13 * hash) + BaseType.GetHashCode();
            hash = (13 * hash) + ChildType.GetHashCode();
            return hash;
        }
    }*/
}