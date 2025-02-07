using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DictionaryCustom<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    [NonSerialized] private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public DictionaryCustom()
    {
        Synchronize();
    }

    public DictionaryCustom(DictionaryCustom<TKey, TValue> dict)
    {
        keys = dict.keys;
        values = dict.values;
        dictionary = dict.dictionary;
        Synchronize();
    }
    public void Synchronize()
    {
        dictionary.Clear();

        for (int i = 0; i < keys.Count && i < values.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }
    public void Add(TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
            throw new ArgumentException("An item with the same key already exists.");

        keys.Add(key);
        values.Add(value);
        dictionary.Add(key, value);
    }
    public bool Remove(TKey key)
    {
        if (!dictionary.TryGetValue(key, out TValue value))
            return false;

        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            keys.RemoveAt(index);
            values.RemoveAt(index);
        }
        dictionary.Remove(key);
        return true;
    }
    public bool Update(TKey key, TValue value)
    {
        if (!dictionary.ContainsKey(key))
            return false;

        int index = keys.IndexOf(key);
        if (index >= 0)
        {
            values[index] = value;
        }

        dictionary[key] = value;
        return true;
    }
    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }
    public void Clear()
    {
        keys.Clear();
        values.Clear();
        dictionary.Clear();
    }
    public List<KeyValuePair<TKey, TValue>> GetAllData()
    {
        var data = new List<KeyValuePair<TKey, TValue>>();
        for (int i = 0; i < keys.Count; i++)
        {
            data.Add(new KeyValuePair<TKey, TValue>(keys[i], values[i]));
        }
        return data;
    }
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    public TValue this[TKey key]
    {
        get => dictionary[key];
        set => Update(key, value);
    }
    public int Count => dictionary.Count;
    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }
    public bool ContainsValue(TValue value)
    {
        return dictionary.ContainsValue(value);
    }
}