using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;

namespace Paralect.Machine
{
    /// <summary>
    /// Header - is a simple collection of key-value pairs.
    ///  
    /// Yeah, i know. You think that this implementation is so silly and dumb.
    /// 
    /// But:
    ///  1) Just created empty Header is perfectly serialized to 0 (zero!) bytes by protobuf. 
    ///  2) There is no boxing/unboxing in realtime, as in case of single (String, Object) dictionary.
    ///  3) There is no needs for parsing (at least for supported types), as in case of single (Sting, String) dictionary.
    /// 
    /// </summary>
    [ProtoContract]
    public class Header
    {
        /// <summary>
        /// Metadata dictionaries for 6 most common types. 
        /// Although methods are public (to make Protobuf happy), they shouldn't be used in user code.
        /// Consider Metdata
        /// </summary>
        [ProtoMember(1)] public Dictionary<String, String>   _StringMetadata   { get; set; }
        [ProtoMember(2)] public Dictionary<String, Int32>    _Int32Metadata    { get; set; }
        [ProtoMember(3)] public Dictionary<String, Int64>    _Int64Metadata    { get; set; }
        [ProtoMember(4)] public Dictionary<String, Guid>     _GuidMetadata     { get; set; }
        [ProtoMember(5)] public Dictionary<String, Boolean>  _BooleanMetadata  { get; set; }
        [ProtoMember(6)] public Dictionary<String, DateTime> _DateTimeMetadata { get; set; }

        /// <summary>
        /// Merged metadata
        /// </summary>
        private Dictionary<String, Object> _mergedMetadata;

        /// <summary>
        /// Should not be serialized. 
        /// TODO: Maybe this should be a method and we should move responsibility of caching to caller side?
        /// </summary>
        public Dictionary<String, Object> Metadata
        {
            get
            {
                if (_mergedMetadata == null)
                {
                    _mergedMetadata = new Dictionary<string, object>(GetNumberOfKeyValuePairs());

                    /* 1 */  AddToMergedMetdata(_StringMetadata);
                    /* 2 */  AddToMergedMetdata(_Int32Metadata);
                    /* 3 */  AddToMergedMetdata(_Int64Metadata);
                    /* 4 */  AddToMergedMetdata(_GuidMetadata);
                    /* 5 */  AddToMergedMetdata(_BooleanMetadata);
                    /* 6 */  AddToMergedMetdata(_DateTimeMetadata);
                }

                return _mergedMetadata;
            }
        }

        private void AddToMergedMetdata<TType>(IDictionary<String, TType> from)
        {
            if (from == null)
                return;

            foreach (var keyValuePair in from) _mergedMetadata.Add(keyValuePair.Key, keyValuePair.Value);
        }

        /// <summary>
        /// Calculates total number of key-value pairs in all dictionaries
        /// </summary>
        /// <returns></returns>
        private Int32 GetNumberOfKeyValuePairs()
        {
            return
                /* 1 */  (_StringMetadata   == null ? 0 : _StringMetadata.Count  ) +
                /* 2 */  (_Int32Metadata    == null ? 0 : _Int32Metadata.Count   ) +
                /* 3 */  (_Int64Metadata    == null ? 0 : _Int64Metadata.Count   ) +
                /* 4 */  (_GuidMetadata     == null ? 0 : _GuidMetadata.Count    ) +
                /* 5 */  (_BooleanMetadata  == null ? 0 : _BooleanMetadata.Count ) +
                /* 6 */  (_DateTimeMetadata == null ? 0 : _DateTimeMetadata.Count);
        }

        public Header()
        {
            // We are not initializing all dictionaries in constructor. 
            // So they can be null if accessed directly.
        }

        /*
        ** "Set" methods. All "Set" methods overwriting previous value, if such exist.
        */ 

        public void Set(String key, String value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            _StringMetadata[key] = value;
        }

        public void Set(String key, Int32 value)
        {
            if (key == null) throw new ArgumentNullException("key");

            _Int32Metadata[key] = value;
        }

        public void Set(String key, Int64 value)
        {
            if (key == null) throw new ArgumentNullException("key");

            _Int64Metadata[key] = value;
        }

        public void Set(String key, Guid value)
        {
            if (key == null) throw new ArgumentNullException("key");

            _GuidMetadata[key] = value;
        }

        public void Set(String key, Boolean value)
        {
            if (key == null) throw new ArgumentNullException("key");

            _BooleanMetadata[key] = value;
        }

        public void Set(String key, DateTime value)
        {
            if (key == null) throw new ArgumentNullException("key");

            _DateTimeMetadata[key] = value;
        }

        /*
        ** "Get" methods. All "Get" methods throws exception if no such key was found.
        */ 

        public String GetString(String key)
        {
            return _StringMetadata[key];
        }

        public Int32 GetInt32(String key)
        {
            return _Int32Metadata[key];
        }

        public Int64 GetInt64(String key)
        {
            return _Int64Metadata[key];
        }

        public Guid GetGuid(String key)
        {
            return _GuidMetadata[key];
        }

        public Boolean GetBoolean(String key)
        {
            return _BooleanMetadata[key];
        }

        public DateTime GetDateTime(String key)
        {
            return _DateTimeMetadata[key];
        }

        /*
        ** "Contains" methods
        */

        public Boolean ContainsString(String key)
        {
            return _StringMetadata.ContainsKey(key);
        }

        public Boolean ContainsInt32(String key)
        {
            return _Int32Metadata.ContainsKey(key);
        }

        public Boolean ContainsInt64(String key)
        {
            return _Int64Metadata.ContainsKey(key);
        }

        public Boolean ContainsGuid(String key)
        {
            return _GuidMetadata.ContainsKey(key);
        }

        public Boolean ContainsBoolean(String key)
        {
            return _BooleanMetadata.ContainsKey(key);
        }

        public Boolean ContainsDateTime(String key)
        {
            return _DateTimeMetadata.ContainsKey(key);
        }

        /*
        ** "TryGet" methods
        */

        public Boolean TryGetString(String key, out String value)
        {
            return _StringMetadata.TryGetValue(key, out value);
        }

        public Boolean TryGetInt32(String key, out Int32 value)
        {
            return _Int32Metadata.TryGetValue(key, out value);
        }

        public Boolean TryGetInt64(String key, out Int64 value)
        {
            return _Int64Metadata.TryGetValue(key, out value);
        }

        public Boolean TryGetGuid(String key, out Guid value)
        {
            return _GuidMetadata.TryGetValue(key, out value);
        }

        public Boolean TryGetBoolean(String key, out Boolean value)
        {
            return _BooleanMetadata.TryGetValue(key, out value);
        }

        public Boolean TryGetDateTime(String key, out DateTime value)
        {
            return _DateTimeMetadata.TryGetValue(key, out value);
        }

        /*
        ** TODO: Clone() method
        */

    }
}