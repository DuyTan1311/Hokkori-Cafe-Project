using UnityEngine;
using System.Collections.Generic;

/* this script is used to manage the pools and execute the spawn/despawn
 requests only */
public class SpawnSystem : MonoBehaviour
{
    private Dictionary<string, object> pools = new();

    /* this method is used to create a specific pool of one type of object
     and add it to the dictionary, if the pool already exists, do nothing */
    public void RegisterPool<T>(string key, T prefab, int initialSize) where T : Component
    {
        if (pools.ContainsKey(key))
        {
            return;
        }
        pools[key]= new ObjectPool<T>(prefab, initialSize, transform);
    }

    /* this method is called by other script when it needs to spawn an object,
     the method will check if the pool is exist, and if yes, called the spawn method
    from the object pool */
    public T Spawn<T>(string key, Vector3 position, Quaternion rotation) where T : Component
    {
        if(!pools.TryGetValue(key, out var pool))
        {
            Debug.LogError($"Pool {key} not found");
            return null;
        }

        return ((ObjectPool<T>)pool).Spawn(position, rotation);
    }

    /* this method is the same with spawn, but instead of spawn, it despawns */
    public void Despawn<T>(string key, T obj) where T : Component
    {
        if(!pools.TryGetValue(key, out var pool))
        {
            return;
        }

        ((ObjectPool<T>)pool).Despawn(obj);
    }
}
