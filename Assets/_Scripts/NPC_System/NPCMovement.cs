using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent (typeof(NavMeshObstacle))]
public class NPCMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();

        obstacle.enabled = false;
        obstacle.carving = true;
        agent.enabled = true;
    }

    public void MoveTo(Vector3 target)
    {
        obstacle.enabled = false;
        agent.enabled = true;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(target);
        }
    }

    public void SitDown()
    {
        agent.enabled = false;
        obstacle.enabled = true;
    }

    public bool HasReached()
    {
        if (!agent.enabled)
        {
            return true;
        }

        if(agent.pathPending == true)
        {
            return false;
        }

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
