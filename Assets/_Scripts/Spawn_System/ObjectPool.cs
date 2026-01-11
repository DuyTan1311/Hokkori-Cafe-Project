using UnityEngine;
using System.Collections.Generic;

public class ObjectPool<T> where T : Component
{
    private Queue<T> pool = new();
    private T prefab;
    private Transform parent;

    /* this is a constructor used for create a pool, when used,
     set the input prefab and parent to the pool and create numbers of prefab,
    and put it into the queue */
    public ObjectPool(T prefab, int initialSIze, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for(int i=0;i < initialSIze; i++)
        {
            CreateNew();
        }
    }

    /* this method is used for creating a new object for the pool,
     when used, instantiate a prefab, disable it and put it into the queue */
    private T CreateNew()
    {
        T obj = Object.Instantiate(prefab, parent);
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
        return obj;
    }

    /* this method is used to spawn object, can be called by SpawnSystem,
     when called, get an object from the pool, set position and rotation base on input,
    enable it, then check if it's a poolable object to call OnSpawn, if there's no object in
    the pool, create a new ones */
    public T Spawn(Vector3 positon, Quaternion rotation)
    {
        if(pool.Count == 0)
        {
            CreateNew();
        }
        T obj = pool.Dequeue();
        obj.transform.SetPositionAndRotation(positon, rotation);
        obj.gameObject.SetActive(true);

        if(obj.TryGetComponent(out IPoolable poolable))
        {
            poolable.OnSpawn();
        }

        return obj;
    }

    /* this method is called by SpawnSystem to despawn an object by putting
     an input refers to the object that has to be despawned, the method will check
    its IPoolable and call OnDespawn, then put the object back to the queue to reuse */
    public void Despawn(T obj)
    {
        if(obj.TryGetComponent(out IPoolable poolable))
        {
            poolable.OnDespawn();
        }

        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
