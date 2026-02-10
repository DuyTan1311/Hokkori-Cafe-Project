using UnityEngine;

/* this script is used to store info about the seat */
public class Seat : MonoBehaviour
{
    public bool isOccupied = false;

    [SerializeField] private Transform sitPoint;

    public Vector3 GetSitPosition()
    {
        if(sitPoint != null)
        {
            return sitPoint.position;
        }
        else
        {
            return transform.position;
        }
    }

    public void Occupy()
    {
        isOccupied = true;
    }

    public void Release()
    {
        isOccupied = false;
    }
}
