﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Paralect.Machine.Messages
{
    public class MessageFactory
    {
        /// <summary>
        /// Different kinds of maps to insure uniqueness of Message Tag, Message Proto Hierarchy Tag and Message Type.
        /// </summary>
        private readonly ConcurrentDictionary<Guid, MessageDefinition> _tagToMessageType = new ConcurrentDictionary<Guid, MessageDefinition>();
        private readonly ConcurrentDictionary<Type, MessageDefinition> _typeToMessageType = new ConcurrentDictionary<Type, MessageDefinition>();
        private readonly ConcurrentDictionary<Int32, MessageDefinition> _protoHierarchyTagToMessageType = new ConcurrentDictionary<Int32, MessageDefinition>();

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

        public Func<Guid, Type> TagToTypeResolver
        {
            get { return tag => GetMessageType(tag); }
        }

        public Func<Type, Guid> TypeToTagResolver
        {
            get { return type => GetMessageTag(type); }
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
        private void Initialize(IEnumerable<Type> messageTypes)
        {
            foreach (var type in messageTypes)
            {
                var currentType = type;
                var baseType = type.BaseType;

                while (currentType != typeof(object) && currentType != null)
                {
                    ProcessMessageType(currentType);

                    currentType = baseType;
                    baseType = baseType.BaseType;
                }
            }            
        }

        private void ProcessMessageType(Type type)
        {
            var messageAttribute = GetSingleAttribute<MessageAttribute>(type);

            if (messageAttribute == null)
                throw new MessageTagNotSpecified(type);

            if (_typeToMessageType.ContainsKey(type))
                return; // throw new MessageTypeAlreadyRegistered(type);

            if (_tagToMessageType.ContainsKey(messageAttribute.Tag))
                throw new MessageTagAlreadyRegistered(messageAttribute.Tag, type, _tagToMessageType[messageAttribute.Tag].Type);

            //                if (_protoHierarchyTagToMessageType.ContainsKey(messageAttribute.ProtoHierarchyTag))
            //                  throw new MessageProtoHierarchyTagCollision(messageAttribute.ProtoHierarchyTag, type, 
            //                    _protoHierarchyTagToMessageType[messageAttribute.ProtoHierarchyTag].Type);

            var messageType = new MessageDefinition(type, messageAttribute.Tag/*, messageAttribute.ProtoHierarchyTag*/);

            _tagToMessageType[messageAttribute.Tag] = messageType;
            _typeToMessageType[type] = messageType;
            //                _protoHierarchyTagToMessageType[messageAttribute.ProtoHierarchyTag] = messageType;            
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