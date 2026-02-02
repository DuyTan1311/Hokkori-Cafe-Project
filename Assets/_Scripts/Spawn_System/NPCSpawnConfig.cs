using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/* this scriptable object is used to store data as a preset about
 what kinds of npc can be spawn, spawn interval, spawn rate depends on the gameplay */
[CreateAssetMenu(fileName = "NewSpawnConfig", menuName = "NPC Spawn Config")]
public class NPCSpawnConfig : ScriptableObject
{
    public List<NPCSpawnData> npcPool; // Spawnable npc list
    public float minSpawnInterval = 5f;
    public float maxSpawnInterval = 15f;

    [System.Serializable]
    public class NPCSpawnData
    {
        public string poolKey;
        public NPCController prefab;
        public int initialPoolSize = 5;
        public int spawnWeight; // Spawn rate
    }

    public string GetRandomNPCKey()
    {
        // insert spawn rate modify logic here
        return npcPool[Random.Range(0, npcPool.Count)].poolKey;
    }
}
