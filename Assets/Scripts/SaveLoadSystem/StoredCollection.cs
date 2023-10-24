using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class StoredCollection<T> : ICollection<T>
{
    [SerializeField]
    private List<T> items;
    private ISaveLoad saveLoad;
    
    public event Action<List<T>> OnCollectionChanged;

    public StoredCollection(ISaveLoad saveLoad)
    {
        items = new List<T>();
        this.saveLoad = saveLoad;
    }

    public StoredCollection(List<T> collection, ISaveLoad saveLoad)
    {
        items = collection;
        this.saveLoad = saveLoad;
    }

    public T this[int index]
    {
        get => items[index];
        set => SetValue(value, index, false);
    }

    public void SetValue(T value, int index, bool forceSave = false)
    {
        items[index] = value;
        Save(forceSave);
    }

    public T GetValue(int index)
    {
        return items[index];
    }

    private void Save(bool forceSave)
    {
        saveLoad.Save();
        OnCollectionChanged?.Invoke(items);
    }

    public void Add(T item)
    {
        Add(item, false);
    }

    public void Add(T item, bool forceSave = false)
    {
        items.Add(item);
        Save(forceSave);
    }

    public void AddRange(List<T> collection, bool forceSave = true)
    {
        items.AddRange(collection);
        Save(forceSave);
    }

    public bool Remove(T item)
    {
        return Remove(item, false);
    }

    public bool Remove(T item, bool forceSave = false)
    {
        var removed = items.Remove(item);
        if (removed)
        {
            Save(forceSave);
        }
        return removed;
    }

    public void Clear()
    {
        Clear(false);
    }

    public void Clear(bool forceSave)
    {
        items.Clear();
        Save(forceSave);
    }

    public bool Contains(T item)
    {
        return items.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        items.CopyTo(array, arrayIndex);
    }

    public int Count => items.Count;
    public bool IsReadOnly => false;

    public IEnumerator<T> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return items.GetEnumerator();
    }
    
    public void SetSaveLoad(ISaveLoad saveLoad)
    {
        this.saveLoad = saveLoad;
    }
}
