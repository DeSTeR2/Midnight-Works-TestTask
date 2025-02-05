using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DictionaryCustom<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // Non-serialized runtime dictionary for fast lookups
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

    /// <summary>
    /// Synchronizes the runtime dictionary with the serialized lists.
    /// </summary>
    public void Synchronize()
    {
        dictionary.Clear();

        for (int i = 0; i < keys.Count && i < values.Count; i++)
        {
            dictionary[keys[i]] = values[i];
        }
    }

    /// <summary>
    /// Adds a new key-value pair.
    /// </summary>
    public void Add(TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
            throw new ArgumentException("An item with the same key already exists.");

        keys.Add(key);
        values.Add(value);
        dictionary.Add(key, value);
    }

    /// <summary>
    /// Removes an item by key.
    /// </summary>
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

    /// <summary>
    /// Updates the value for an existing key.
    /// </summary>
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

    /// <summary>
    /// Gets a value by key.
    /// </summary>
    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    /// <summary>
    /// Clears all data.
    /// </summary>
    public void Clear()
    {
        keys.Clear();
        values.Clear();
        dictionary.Clear();
    }

    /// <summary>
    /// Gets all data as a list of key-value pairs.
    /// </summary>
    public List<KeyValuePair<TKey, TValue>> GetAllData()
    {
        var data = new List<KeyValuePair<TKey, TValue>>();
        for (int i = 0; i < keys.Count; i++)
        {
            data.Add(new KeyValuePair<TKey, TValue>(keys[i], values[i]));
        }
        return data;
    }

    /// <summary>
    /// Enumerator for foreach loops.
    /// </summary>
    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Indexer for easy access.
    /// </summary>
    public TValue this[TKey key]
    {
        get => dictionary[key];
        set => Update(key, value);
    }

    /// <summary>
    /// Gets the count of items.
    /// </summary>
    public int Count => dictionary.Count;

    /// <summary>
    /// Checks if the dictionary contains a key.
    /// </summary>
    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    /// <summary>
    /// Checks if the dictionary contains a value.
    /// </summary>
    public bool ContainsValue(TValue value)
    {
        return dictionary.ContainsValue(value);
    }
}