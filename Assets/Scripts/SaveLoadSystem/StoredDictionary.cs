using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StoredDictionary<TKey, TValue> : IDictionary<TKey, TValue>
{
    [Serializable]
    public class KeyValuePair
    {
        public TKey Key;
        public TValue Value;

        public KeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
    }

    [SerializeField]
    private List<KeyValuePair> items;
    
    private ISaveLoad saveLoad;

    public event Action<Dictionary<TKey, TValue>> OnCollectionChanged;

    public StoredDictionary(ISaveLoad saveLoad)
    {
        items = new List<KeyValuePair>();
        this.saveLoad = saveLoad;
    }

    public TValue this[TKey key]
    {
        get => GetValue(key);
        set => SetValue(key, value);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        var index = GetIndex(key);
        if (index != -1)
        {
            value = items[index].Value;
            return true;
        }
        value = default;
        return false;
    }

    public void Add(TKey key, TValue value)
    {
        Add(key, value, false);
    }

    public void Add(TKey key, TValue value, bool forceSave)
    {
        if (ContainsKey(key))
            throw new ArgumentException("Element with the same key already exists in the Dictionary.", paramName: "key");

        items.Add(new KeyValuePair(key, value));
        Save();
    }

    public bool Remove(TKey key)
    {
        return Remove(key, false);
    }

    public bool Remove(TKey key, bool forceSave)
    {
        if (ContainsKey(key))
        {
            var index = GetIndex(key);
            items.RemoveAt(index);
            Save();
            return true;
        }
        return false;
    }

    public void Clear()
    {
        Clear(false);
    }

    public void Clear(bool forceSave)
    {
        items.Clear();
        Save();
    }

    public bool ContainsKey(TKey key)
    {
        return GetDictionary().ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
        return GetDictionary().ContainsValue(value);
    }

    public IEqualityComparer<TKey> Comparer => GetDictionary().Comparer;

    public int Count => GetDictionary().Count;

    public ICollection<TKey> Keys => GetDictionary().Keys;

    public ICollection<TValue> Values => GetDictionary().Values;

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return GetDictionary().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return GetDictionary().TryGetValue(item.Key, out TValue value) && value.Equals(item.Key);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)GetDictionary()).CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

    private int GetIndex(TKey key)
    {
        return items.FindIndex(x => x.Key.Equals(key));
    }

    private KeyValuePair GetItem(TKey key)
    {
        var index = GetIndex(key);
        var item = items[index];
        return item;
    }

    private TValue GetValue(TKey key)
    {
        var item = GetItem(key);
        return item.Value;
    }

    public void SetValue(TKey key, TValue value)
    {
        if (ContainsKey(key))
        {
            var item = GetItem(key);
            item.Value = value;
            Save();
        }
        else
        {
            Add(key, value, false);
        }
    }

    private Dictionary<TKey, TValue> GetDictionary()
    {
        return items.ToDictionary(x => x.Key, x => x.Value);
    }

    private void Save()
    {
        saveLoad.Save();
        OnCollectionChanged?.Invoke(GetDictionary());
    }
    
    public void SetSaveLoad(ISaveLoad saveLoad)
    {
        this.saveLoad = saveLoad;
    }
}
