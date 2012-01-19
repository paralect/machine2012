using System;
using System.Collections.Generic;
using System.IO;
using Paralect.Machine.Messages;
using ProtoBuf;
using ProtoBuf.Meta;

namespace Paralect.Machine.Serialization
{
    public struct AlreadyRegisteredInHierarchyTypes
    {
        public Type BaseType { get; set; }
        public Type ChildType { get; set; }

        public AlreadyRegisteredInHierarchyTypes(Type baseType, Type childType) : this()
        {
            BaseType = baseType;
            ChildType = childType;
        }

        public override int GetHashCode()
        {
            // More about: http://stackoverflow.com/a/720282/407599
            //             http://www.pcreview.co.uk/forums/writing-own-gethashcode-function-t3182933.html

            int hash = 27;
            hash = (13 * hash) + BaseType.GetHashCode();
            hash = (13 * hash) + ChildType.GetHashCode();
            return hash;            
        }
    }

    public class ProtobufSerializer
    {
        private readonly RuntimeTypeModel _model = TypeModel.Create();

        private readonly Dictionary<AlreadyRegisteredInHierarchyTypes, Boolean> _hierarchy = new Dictionary<AlreadyRegisteredInHierarchyTypes, bool>();

        public void RegisterMessages(IEnumerable<MessageDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                var type = definition.Type;
                var baseType = type.BaseType;

                while (baseType != typeof(object) && baseType != null)
                {
                    var already = new AlreadyRegisteredInHierarchyTypes(baseType, type);

                    if (_hierarchy.ContainsKey(already))
                        break;

                    _model[baseType]
                        .AddSubType(definition.ProtoHierarchyTag, type);

                    _hierarchy.Add(already, true);
                    type = baseType;
                    baseType = baseType.BaseType;
                }
            }
        }

        public byte[] Serialize(Object obj)
        {
            using (MemoryStream ms = new System.IO.MemoryStream())
            {
                _model.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        public TObject Deserialize<TObject>(byte[] data)
        {
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                TObject obj = (TObject)(_model.Deserialize(memoryStream, null, typeof(TObject)));
                return obj;
            }
        }
    }
}