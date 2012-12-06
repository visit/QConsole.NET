using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace QConsole.NET
{
    [Serializable]
    public class CommandsDictionary<K, V> : ISerializable
    {
        Dictionary<K, V> dict = new Dictionary<K, V>();

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (K key in dict.Keys)
            {
                info.AddValue(key.ToString(), dict[key]);
            }
        }

        public void Add(K key, V value)
        {
            dict.Add(key, value);
        }

        public V this[K index]
        {
            set { dict[index] = value; }
            get { return dict[index]; }
        }
    }
}