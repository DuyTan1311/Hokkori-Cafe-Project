using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    NavMeshAgent agent;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] float clickDistance = 100f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    public void ClicktoMove()
    {
        RaycastHit hit;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()), out hit, clickDistance, groundLayer))
        {
            agent.destination = hit.point; 
        }
    }
}
