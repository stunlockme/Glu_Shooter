using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [System.Serializable]   //expose class in inspector.
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private GameObject parent;
    public Transform PoolParent { get { return parent.transform; } private set {; } }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        parent = GameObject.Find("PooledObjects");

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.transform.SetParent(parent.transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    /// <summary>
    /// spawn gameobject from pool.
    /// </summary>
    /// <param name="tag">name of obj in pool</param>
    /// <param name="position">pos of obj to spawn at</param>
    /// <param name="rotation">rotation of obj to spawn with</param>
    /// <param name="parent">parent of obj</param>
    /// <returns>gameobject</returns>
    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        if (!poolDictionary.ContainsKey(tag))
            return null;

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.transform.SetParent(parent);
        objectToSpawn.SetActive(true);
        //IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        //if (pooledObj != null)
        //    pooledObj.Init();

        poolDictionary[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}