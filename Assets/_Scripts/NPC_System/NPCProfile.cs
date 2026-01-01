using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCProfile", menuName = "NPC Profile")]
public class NPCProfile : ScriptableObject
{
    public string NPCName;
    public float waitTime;
}
