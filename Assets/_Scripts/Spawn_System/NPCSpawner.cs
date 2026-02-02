using UnityEngine;

/* This script is used to control when, where and how NPC spawn */
public class NPCSpawner : MonoBehaviour
{
    [SerializeField] private SpawnSystem spawnSystem;
    [SerializeField] private NPCSpawnConfig currentConfig;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform exitPoint;
    [SerializeField] private int maxActiveNPC = 10;

    private float nextSpawnTime;
    private int currentActiveNPC;

    private void Start()
    {
        EnsurePoolsRegistered();
        CalculateNextSpawnTime();
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime) {
            SpawnNPC();
            CalculateNextSpawnTime();
        }
    }

    private void EnsurePoolsRegistered() // this method is temporary, have to change later for decoupling
    {
        foreach(var data in currentConfig.npcPool)
        {
            spawnSystem.RegisterPool(data.poolKey,data.prefab,data.initialPoolSize);
        }
    }

    /* this method is used to calculate next spawn time based on real time,
     it random the next spawn time and add it to real time like setting a reminder */
    void CalculateNextSpawnTime()
    {
        nextSpawnTime = Time.time + Random.Range(currentConfig.minSpawnInterval, currentConfig.maxSpawnInterval);
    }

    /* this method is used to spawn NPC, when called, it get the pool key from the config asset,
     and then get a npc instance from the pool by calling spawn system, if the number of
    active NPC is larger than max, stop spawning */
    void SpawnNPC()
    {
        if(currentActiveNPC >= maxActiveNPC)
        {
            return;
        }
        string key = currentConfig.GetRandomNPCKey();
        var npc = spawnSystem.Spawn<NPCController>(key, spawnPoint.position, spawnPoint.rotation);
        if(npc != null)
        {
            currentActiveNPC++;
        }
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
