using Fusion;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : NetworkBehaviour
{
    [Serializable]
    public class Pool
    {
        public string tag;
        public NetworkPrefabRef prefab;
        public int size;
    }
    [SerializeField] List<Pool> pools = new List<Pool>();
    private Dictionary<string, Queue<NetworkObject>> poolerDictionary = new Dictionary<string, Queue<NetworkObject>>();

    private bool isItialized = false;
    private void Update()
    {
        if (isItialized) return;

        InitializePools();
    }
    private void InitializePools()
    {
        if (HasStateAuthority)
        {
            isItialized = true;
            foreach (Pool pool in pools)
            {
                Queue<NetworkObject> queue = new Queue<NetworkObject>();
                for (int i = 0; i < pool.size; i++)
                {
                    NetworkObject obj = Runner.Spawn(pool.prefab, Vector3.zero, Quaternion.identity);
                    obj.gameObject.SetActive(false);
                    obj.transform.SetParent(transform);
                    queue.Enqueue(obj);
                }
                poolerDictionary.Add(pool.tag, queue);
            }
        }
    }

    public NetworkObject SpawnPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolerDictionary.ContainsKey(tag))
        {
            Debug.LogError("Don't have object with tag : " + tag);
            return null;
        }

        NetworkObject obj = poolerDictionary[tag].Dequeue();
        poolerDictionary[tag].Enqueue(obj);

        if (!obj.gameObject.activeInHierarchy)
        {
            obj.gameObject.SetActive(true);

            obj.transform.position = position;

            obj.transform.rotation = rotation;
        }
        return obj;
    }
}
