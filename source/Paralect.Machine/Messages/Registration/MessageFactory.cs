using System;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public class MessageFactory
    {
        /// <summary>
        /// (MessageTag --> MessageType) and (MessageType --> MessageTag) maps
        /// </summary>
        private readonly Dictionary<Guid, MessageDefinition> _tagToMessageType = new Dictionary<Guid, MessageDefinition>(100);
        private readonly Dictionary<Type, MessageDefinition> _typeToMessageType = new Dictionary<Type, MessageDefinition>(100);

        private readonly Dictionary<Int32, MessageDefinition> _protoHierarchyTags = new Dictionary<Int32, MessageDefinition>(100);

        /// <summary>
        /// All registered Identity Types
        /// </summary>
        public IEnumerable<Type> MessageTypes
        {
            get { return _typeToMessageType.Keys; }
        }

        public IEnumerable<MessageDefinition> MessageDefinitions
        {
            get { return _typeToMessageType.Values; }
        }

        /// <summary>
        /// Creates MessageFactory with specified set of message types.
        /// </summary>
        public MessageFactory(IEnumerable<Type> messageTypes)
        {
            Initialize(messageTypes);
        }

        /// <summary>
        /// Creates MessageFactory with specified set of message types.
        /// </summary>
        public MessageFactory(params Type[] identityTypes)
        {
            Initialize(identityTypes);
        }

        /// <summary>
        /// Initializes Factory state. Checks that Message tags are unique.
        /// </summary>
        private void Initialize(IEnumerable<Type> identityTypes)
        {
            foreach (var type in identityTypes)
            {
                var messageAttribute = GetSingleAttribute<MessageAttribute>(type);

                if (messageAttribute == null)
                    continue;

                if (_typeToMessageType.ContainsKey(type))
                    throw new MessageTypeAlreadyRegistered(type);

                if (_tagToMessageType.ContainsKey(messageAttribute.Tag))
                    throw new MessageTagAlreadyRegistered(messageAttribute.Tag, type, _tagToMessageType[messageAttribute.Tag].Type);

                if (_protoHierarchyTags.ContainsKey(messageAttribute.ProtoHierarchyTag))
                    throw new MessageProtoHierarchyTagCollision(messageAttribute.ProtoHierarchyTag, type, 
                        _protoHierarchyTags[messageAttribute.ProtoHierarchyTag].Type);
                    
                var messageType = new MessageDefinition(type, messageAttribute.Tag, messageAttribute.ProtoHierarchyTag);

                _tagToMessageType[messageAttribute.Tag] = messageType;
                _typeToMessageType[type] = messageType;
                _protoHierarchyTags[messageAttribute.ProtoHierarchyTag] = messageType;
            }            
        }

        /// <summary>
        /// Returns entity tag by Identity type
        /// </summary>
        public Guid GetMessageTag(Type identityType)
        {
            return _typeToMessageType[identityType].Tag;
        }

        /// <summary>
        /// Returns Identity Type by enitity tag value
        /// </summary>
        public Type GetMessageType(Guid tag)
        {
            return _tagToMessageType[tag].Type;
        }

        /// <summary>
        /// Returns Identity Type by enitity tag value
        /// </summary>
        public MessageDefinition GetMessageDefinition(Guid tag)
        {
            return _tagToMessageType[tag];
        }

        /// <summary>
        /// Returns Identity Type by enitity tag value
        /// </summary>
        public MessageDefinition GetMessageDefinition(Type type)
        {
            return _typeToMessageType[type];
        }

        #region Reflection helpers

        /// <summary>
        /// Returns attribute instance for specified type. Will return default type value if not found or not single.
        /// </summary>
        private static TAttribute GetSingleAttribute<TAttribute>(Type type)
        {
            var identities = type.GetCustomAttributes(typeof(TAttribute), false);

            if (identities.Length != 1)
                return default(TAttribute);

            if (!(identities[0] is TAttribute))
                return default(TAttribute);

            return (TAttribute)identities[0];
        }

        #endregion
    }
}