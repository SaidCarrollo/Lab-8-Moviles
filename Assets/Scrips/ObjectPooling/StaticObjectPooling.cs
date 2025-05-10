
using System;
using System.Collections.Generic;
using UnityEngine;

public class StaticObjectPooling : MonoBehaviour
{
    public static StaticObjectPooling Instance { get; private set; }

    [System.Serializable]
    public class PoolConfig
    {
        public string poolID;
        public PoolObject prefab;
        public int initialSize = 10;
        public bool expandable = false;
    }

    [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();
    private Dictionary<string, Queue<PoolObject>> pools = new Dictionary<string, Queue<PoolObject>>();
    private Dictionary<string, PoolConfig> configLookup = new Dictionary<string, PoolConfig>();

    public static event Action<PoolObject> OnObjectGet;
    public static event Action<PoolObject> OnObjectReturn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        foreach (var config in poolConfigs)
        {
            var queue = new Queue<PoolObject>();
            var parent = new GameObject(config.poolID + " Pool").transform;
            parent.SetParent(transform);

            for (int i = 0; i < config.initialSize; i++)
            {
                var obj = Instantiate(config.prefab, parent);
                obj.PoolID = config.poolID;
                obj.Initialize(this);
                obj.Deactivate();
                queue.Enqueue(obj);
            }

            pools.Add(config.poolID, queue);
            configLookup.Add(config.poolID, config);
        }
    }

    public PoolObject GetObject(string poolID)
    {
        if (!pools.ContainsKey(poolID))
        {
            Debug.LogError($"Pool {poolID} not configured!");
            return null;
        }

        var queue = pools[poolID];
        if (queue.Count > 0)
        {
            var obj = queue.Dequeue();
            obj.EnableObject();
            OnObjectGet?.Invoke(obj);
            return obj;
        }

        var config = configLookup[poolID];
        if (config.expandable)
        {
            var parent = transform.Find(config.poolID + " Pool");
            var newObj = Instantiate(config.prefab, parent);
            newObj.PoolID = poolID;
            newObj.Initialize(this);
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
        pools[obj.PoolID].Enqueue(obj);
        OnObjectReturn?.Invoke(obj);
    }
}