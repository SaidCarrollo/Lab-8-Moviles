// DynamicObjectPooling.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectPooling : MonoBehaviour
{
    public static DynamicObjectPooling Instance { get; private set; }

    private class PoolData
    {
        public Queue<PoolObject> queue;
        public Transform parent;
        public bool expandable;
    }

    private Dictionary<string, PoolData> pools = new Dictionary<string, PoolData>();

    public static event Action<PoolObject> OnObjectGet;
    public static event Action<PoolObject> OnObjectReturn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreatePool(string poolID, PoolObject prefab, int initialSize = 10, bool expandable = true)
    {
        if (!pools.ContainsKey(poolID))
        {
            var queue = new Queue<PoolObject>();
            var parent = new GameObject(poolID + " Pool").transform;
            parent.SetParent(transform);

            for (int i = 0; i < initialSize; i++)
            {
                var obj = Instantiate(prefab, parent);
                obj.PoolID = poolID;
                obj.Initialize();
                obj.Deactivate();
                queue.Enqueue(obj);
            }

            pools[poolID] = new PoolData
            {
                queue = queue,
                parent = parent,
                expandable = expandable
            };
        }
    }

    public PoolObject GetObject(string poolID)
    {
        if (!pools.ContainsKey(poolID))
        {
            Debug.LogError($"Pool {poolID} not found!");
            return null;
        }

        var pool = pools[poolID];
        if (pool.queue.Count > 0)
        {
            var obj = pool.queue.Dequeue();
            obj.EnableObject();
            OnObjectGet?.Invoke(obj);
            return obj;
        }

        if (pool.expandable)
        {
            var prefab = pool.queue.Peek(); // Safe since we created pool with at least one object
            var newObj = Instantiate(prefab, pool.parent);
            newObj.PoolID = poolID;
            newObj.Initialize();
            newObj.EnableObject();
            OnObjectGet?.Invoke(newObj);
            return newObj;
        }

        Debug.LogWarning($"Pool {poolID} is empty and not expandable!");
        return null;
    }

    public void ReturnObject(PoolObject obj)
    {
        obj.Deactivate();
        pools[obj.PoolID].queue.Enqueue(obj);
        OnObjectReturn?.Invoke(obj);
    }
}