using UnityEngine;

/* This script is used to control when, where and how NPC spawn */
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private SpawnSystem spawnSystem;
    [SerializeField] private NPCSpawnConfig currentConfig;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform exitPoint;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime) {
            SpawnNPC();
            CalculateNextSpawnTime();
        }
    }

    /* this method is used to calculate next spawn time based on real time,
     it random the next spawn time and add it to real time like setting a reminder */
    void CalculateNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(currentConfig.minSpawnInterval, currentConfig.maxSpawnInterval);
    }

    /* this method is used to spawn NPC, when called, it get the pool key from the config asset,
     and then get a npc instance from the pool by calling spawn system */
    void SpawnNPC()
    {
        string key = currentConfig.GetRandomNPCKey();
        var npc = spawnSystem.Spawn<NPCController>(key, spawnPoint.position, spawnPoint.rotation);
    }

    public void DespawnNPC(string key, NPCController npc)
    {
        spawnSystem.Despawn(key, npc);
    }

    /* this method is used to change config, can be use by DayManager or other system
     to change how NPC spawn based on day periods or special kinds of day */
    public void ChangeConfig(NPCSpawnConfig newConfig)
    {
        currentConfig = newConfig;
    }
}
