using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectsPool<T> where T : UnityEngine.Object, IPooledObject
{
    private T pooledObjectPrefab;
    private readonly Stack<T> pool;

    public GameObjectsPool(T pooledObjectPrefab,int pooledElemetsCount)
    {
        this.pooledObjectPrefab = pooledObjectPrefab;
        pool = new Stack<T>();

        int countOfUnitsToGen = pooledElemetsCount;
        while (countOfUnitsToGen > 0)
        {
            IncreasePoolByOne();
            countOfUnitsToGen--;
        }
    }

    public T Get()
    {
        if (pool.Count == 0) IncreasePoolByOne();
        return pool.Pop();
    }

    public void Return(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));
        obj.ResetAndDeactivate();
        pool.Push(obj);
    }

    public void IncreasePoolByOne()
    {
        T createdElement = GameObject.Instantiate(pooledObjectPrefab);
        createdElement.Init();
        pool.Push(createdElement);
    }
}
