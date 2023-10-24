using System;
using UnityEngine;

[Serializable]
public class StoredValue<T>
{
    [SerializeField] private T value;

    private ISaveLoad saveLoad;

    public T Value
    {
        get => value;
        set
        {
            if (!this.value.Equals(value))
            {
                this.value = value;
                saveLoad?.Save();
                //SaveLoadSystem.Instance.Save();
                OnValueChanged?.Invoke(value);
            }
        }
    }

    public event Action<T> OnValueChanged;

    public StoredValue(ISaveLoad saveLoad)
    {
        this.saveLoad = saveLoad;
        try
        {
            value = Activator.CreateInstance<T>();
        }
        catch (MissingMethodException)
        {
            value = default(T);
        }
    }

    public StoredValue(T value, ISaveLoad saveLoad)
    {
        this.value = value;
        this.saveLoad = saveLoad;
    }

    public void SetSaveLoad(ISaveLoad saveLoad)
    {
        this.saveLoad = saveLoad;
    }
}