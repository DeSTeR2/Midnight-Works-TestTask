using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    protected Queue<T> pool = new Queue<T>();

    public Action OnPoolIsEmpty;

    public virtual void AddObject(T obj) => pool.Enqueue(obj);
    public virtual T GetObject()
    {
        if (pool.Count == 0) OnPoolIsEmpty?.Invoke();
        return pool.Dequeue();
    }
}
