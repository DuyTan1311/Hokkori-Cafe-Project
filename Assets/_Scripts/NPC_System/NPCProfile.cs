using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewNPCProfile", menuName = "NPC Profile")]
public class NPCProfile : ScriptableObject
{
    public string NPCName;
    public float waitTime;
    public float drinkDuration;
}
