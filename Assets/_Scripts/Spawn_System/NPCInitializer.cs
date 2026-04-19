using UnityEngine;

public class NPCInitializer : MonoBehaviour
{
    [SerializeField] private NPCSpawnEvent onNPCSpawned;
    [SerializeField] private SeatManager seatManager;
    [SerializeField] private Transform exitPoint;

    private void OnEnable()
    {
        onNPCSpawned.Raised += HandleNPCSpawned;
    }

    private void OnDisable()
    {
        onNPCSpawned.Raised -= HandleNPCSpawned;
    }

    void HandleNPCSpawned(NPCController npc)
    {
        var behavior = npc.GetComponent<NPCBehavior>();
        behavior.Init(seatManager, exitPoint);
    }
}
