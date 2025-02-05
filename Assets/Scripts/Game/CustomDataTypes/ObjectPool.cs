using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CustomSystems
{
    public class ObjectPool<T>
    {
        protected Queue<T> pool = new Queue<T>();

        public Action<ObjectPool<T>> OnPoolIsEmpty;
        public T Object;

        public ObjectPool(T Object)
        {
            this.Object = Object;
        }

        public ObjectPool()
        {

        }

        public virtual void AddObject(T obj) => pool.Enqueue(obj);
        public virtual T GetObject()
        {
            if (pool.Count == 0) OnPoolIsEmpty?.Invoke(this);
            return pool.Dequeue();
        }
    }
}
