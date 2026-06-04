using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableKeyValuePair<K, T>
{
    public K Key;
    public T Value;

    public SerializableKeyValuePair(K key, T value)
    {
        Key = key;
        Value = value;
    }
}

[Serializable]
public class SerializableDictionary<K, T>
{
    public List<SerializableKeyValuePair<K, T>> Pairs = new List<SerializableKeyValuePair<K, T>>();

    public T this[K key]
    {
        get
        {
            var pair = Pairs.Find(p => EqualityComparer<K>.Default.Equals(p.Key, key));
            return pair != null ? pair.Value : default;
        }
        set
        {
            var pair = Pairs.Find(p => EqualityComparer<K>.Default.Equals(p.Key, key));

            if (pair != null)
            {
                pair.Value = value;
            }
            else
            {
                Pairs.Add(new SerializableKeyValuePair<K, T>(key, value));
            }
        }
    }

    public bool ContainsKey(K key)
    {
        return Pairs.Exists(p => EqualityComparer<K>.Default.Equals(p.Key, key));
    }

    public void Remove(K key)
    {
        Pairs.RemoveAll(p => EqualityComparer<K>.Default.Equals(p.Key, key));
    }   

    public bool TryGetValue(K key, out T value)
    {
        var pair = Pairs.Find(p => EqualityComparer<K>.Default.Equals(p.Key, key));
        if (pair != null)
        {
            value = pair.Value;
            return true;
        }
        value = default;
        return false;
    }

    public void Clear()
    {
        Pairs.Clear();
    }

    public List<K> Keys => Pairs.ConvertAll(p => p.Key);
    public List<T> Values => Pairs.ConvertAll(p => p.Value);

    public int Count => Pairs.Count;
}