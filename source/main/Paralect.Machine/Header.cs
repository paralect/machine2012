using System;
using System.Collections.Generic;
using ProtoBuf;

namespace Paralect.Machine
{
    /// <summary>
    /// Header is simply a key-value collection designed to be:
    ///   1) Well serialized by ProtoBuf (size of empty Header is 0 (zero) bytes)
    ///   2) To eliminate boxing/unboxing in runtime, for supported value types (comparing with single Dictionary(String, Object))
    ///   3) To eliminate parsing in runtime (comparing with single Dictionary(String, String)
    /// 
    /// Value type can be one of the following: String, Int32, Int64, Guid, Boolean and DateTime.
    /// </summary>
    [ProtoContract]
    public class Header
    {
        [ProtoMember(1)]
        public Dictionary<String, IHeaderFieldValue> Fields { get; set; }

        public Header()
        {
            Fields = new Dictionary<string, IHeaderFieldValue>();
        }

        /*
        ** "Set" methods. All "Set" methods overwriting previous value, if it already exists.
        */

        public void Set(String key, String value)
        {
            if (key == null) throw new ArgumentNullException("key");
            if (value == null) throw new ArgumentNullException("value");

            Fields[key] = new HeaderFieldValue<String>(value);
        }

        public void Set(String key, Int32 value)
        {
            if (key == null) throw new ArgumentNullException("key");

            Fields[key] = new HeaderFieldValue<Int32>(value);
        }

        public void Set(String key, Int64 value)
        {
            if (key == null) throw new ArgumentNullException("key");

            Fields[key] = new HeaderFieldValue<Int64>(value);
        }

        public void Set(String key, Guid value)
        {
            if (key == null) throw new ArgumentNullException("key");

            Fields[key] = new HeaderFieldValue<Guid>(value); ;
        }

        public void Set(String key, Boolean value)
        {
            if (key == null) throw new ArgumentNullException("key");

            Fields[key] = new HeaderFieldValue<Boolean>(value); ;
        }

        public void Set(String key, DateTime value)
        {
            if (key == null) throw new ArgumentNullException("key");

            Fields[key] = new HeaderFieldValue<DateTime>(value);
        }

        /*
        ** "Get" methods. All "Get" methods returns null if no  exception if no such key was found.
        */

        public String GetString(String key)
        {
            var value = Fields[key];
            
            if (value == null)
                return null;

            var castedValue = value as HeaderFieldValue<String>;

            if (castedValue == null)
                throw new InvalidCastException();

            return castedValue.Value;
        }

        public Int32 GetInt32(String key)
        {
            var value = Fields[key] as HeaderFieldValue<Int32>;

            if (value == null)
                throw new InvalidCastException();

            return value.Value;
        }

        public Int64 GetInt64(String key)
        {
            var value = Fields[key] as HeaderFieldValue<Int64>;

            if (value == null)
                throw new InvalidCastException();

            return value.Value;
        }

        public Guid GetGuid(String key)
        {
            var value = Fields[key] as HeaderFieldValue<Guid>;

            if (value == null)
                throw new InvalidCastException();

            return value.Value;
        }

        public Boolean GetBoolean(String key)
        {
            var value = Fields[key] as HeaderFieldValue<Boolean>;

            if (value == null)
                throw new InvalidCastException();

            return value.Value;
        }

        public DateTime GetDateTime(String key)
        {
            var value = Fields[key] as HeaderFieldValue<DateTime>;

            if (value == null)
                throw new InvalidCastException();

            return value.Value;
        }

        /*
        ** "Contains" methods
        */

        public Boolean ContainsString(String key)
        {
            var contains =  Fields.ContainsKey(key);
            
            if (!contains)
                return false;

            var value = Fields[key] as HeaderFieldValue<String>;

            // no needs to check explicitly on null string

            if (value == null)
                return false;

            return true;
        }

        public Boolean ContainsInt32(String key)
        {
            var contains = Fields.ContainsKey(key);

            if (!contains)
                return false;

            var value = Fields[key] as HeaderFieldValue<Int32>;
            return value != null;
        }

        public Boolean ContainsInt64(String key)
        {
            var contains = Fields.ContainsKey(key);

            if (!contains)
                return false;

            var value = Fields[key] as HeaderFieldValue<Int64>;
            return value != null;
        }

        public Boolean ContainsGuid(String key)
        {
            var contains = Fields.ContainsKey(key);

            if (!contains)
                return false;

            var value = Fields[key] as HeaderFieldValue<Guid>;
            return value != null;
        }

        public Boolean ContainsBoolean(String key)
        {
            var contains = Fields.ContainsKey(key);

            if (!contains)
                return false;

            var value = Fields[key] as HeaderFieldValue<Boolean>;
            return value != null;
        }

        public Boolean ContainsDateTime(String key)
        {
            var contains = Fields.ContainsKey(key);

            if (!contains)
                return false;

            var value = Fields[key] as HeaderFieldValue<DateTime>;
            return value != null;
        }

        /*
        ** "TryGet" methods
        */
        
        public Boolean TryGetString(String key, out String value)
        {
            IHeaderFieldValue val;
            var result = Fields.TryGetValue(key, out val);

            if (!result)
            {
                value = null;
                return false;
            }

            var field = val as HeaderFieldValue<String>;

            if (field == null)
            {
                value = null;
                return false;
            }

            value = field.Value;
            return true;
        }

        public Boolean TryGetInt32(String key, out Int32 value)
        {
            IHeaderFieldValue val;
            var result = Fields.TryGetValue(key, out val);

            if (!result)
            {
                value = 0;
                return false;
            }

            var field = val as HeaderFieldValue<Int32>;

            if (field == null)
            {
                value = 0;
                return false;
            }

            value = field.Value;
            return true;
        }

        public Boolean TryGetInt64(String key, out Int64 value)
        {
            IHeaderFieldValue val;
            var result = Fields.TryGetValue(key, out val);

            if (!result)
            {
                value = 0;
                return false;
            }

            var field = val as HeaderFieldValue<Int64>;

            if (field == null)
            {
                value = 0;
                return false;
            }

            value = field.Value;
            return true;
        }

        public Boolean TryGetGuid(String key, out Guid value)
        {
            IHeaderFieldValue val;
            var result = Fields.TryGetValue(key, out val);

            if (!result)
            {
                value = default(Guid);
                return false;
            }

            var field = val as HeaderFieldValue<Guid>;

            if (field == null)
            {
                value = default(Guid);
                return false;
            }

            value = field.Value;
            return true;
        }

        public Boolean TryGetBoolean(String key, out Boolean value)
        {
            IHeaderFieldValue val;
            var result = Fields.TryGetValue(key, out val);

            if (!result)
            {
                value = false;
                return false;
            }

            var field = val as HeaderFieldValue<Boolean>;

            if (field == null)
            {
                value = false;
                return false;
            }

            value = field.Value;
            return true;
        }

        public Boolean TryGetDateTime(String key, out DateTime value)
        {
            IHeaderFieldValue val;
            var result = Fields.TryGetValue(key, out val);

            if (!result)
            {
                value = default(DateTime);
                return false;
            }

            var field = val as HeaderFieldValue<DateTime>;

            if (field == null)
            {
                value = default(DateTime);
                return false;
            }

            value = field.Value;
            return true;
        }
         
    }

    [ProtoContract]
    [ProtoInclude(1, typeof(HeaderFieldValue<String>))]
    [ProtoInclude(2, typeof(HeaderFieldValue<Int32>))]
    [ProtoInclude(3, typeof(HeaderFieldValue<Int64>))]
    [ProtoInclude(4, typeof(HeaderFieldValue<Guid>))]
    [ProtoInclude(5, typeof(HeaderFieldValue<Boolean>))]
    [ProtoInclude(6, typeof(HeaderFieldValue<DateTime>))]
    public interface IHeaderFieldValue { }

    [ProtoContract]
    public class HeaderFieldValue<TValueType> : IHeaderFieldValue
    {
        /// <summary>
        /// Field value
        /// </summary>
        [ProtoMember(1)]
        public TValueType Value { get; set; }

        /// <summary>
        /// Protected constructor to make protobuf able to create instance of this type
        /// </summary>
        protected HeaderFieldValue()
        {
        }

        /// <summary>
        /// Creates HeaderFieldValue with specified value
        /// </summary>
        /// <param name="value"></param>
        public HeaderFieldValue(TValueType value)
        {
            Value = value;
        }

        /// <summary>
        /// Returns string representation of value (by calling ToString()) on instance of TValueType
        /// </summary>
        public override string ToString()
        {
            return Value.ToString();
        }
    }

}