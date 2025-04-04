using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool 
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();

    // 외부에서 인스턴스 생성 방지
    private ObjectPool() { }
    public static ObjectPool Instance => instance ??= new ObjectPool();

    public void InitializePool(string key, GameObject prefab, int size)
    {
        if (!objectPool.ContainsKey(key))
        {
            objectPool[key] = new Queue<GameObject>();

            for (int i = 0; i < size; i++)
            {
                GameObject obj = Object.Instantiate(prefab);
                obj.SetActive(false);
                objectPool[key].Enqueue(obj);
            }
        }
    }
    public GameObject GetObject(GameObject prefab)
    {
        EnsureExistKey(prefab.name);

        GameObject obj;
        if (objectPool.ContainsKey(prefab.name) && objectPool[prefab.name].Count > 0)
        {
            obj = objectPool[prefab.name].Dequeue();
        }
        else
        {
            obj = Object.Instantiate(prefab);
        }

        obj.name = prefab.name;
        obj.SetActive(true);
        return obj;
    }

    public void ReturnObject(GameObject obj)
    {
        EnsureExistKey(obj.name);

        obj.SetActive(false);
        objectPool[obj.name].Enqueue(obj);
    }

    public void EnsureExistKey(string key)
    {
        if(objectPool.TryGetValue(key, out Queue<GameObject> queue))
        {
            Debug.Log("Pool for key: " + key + " doesn't exist!");
            queue = new Queue<GameObject>();
            objectPool[key] = queue;
        }
    }
}
