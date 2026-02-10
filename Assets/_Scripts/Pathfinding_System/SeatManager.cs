using UnityEngine;
using System.Collections.Generic;

public class SeatManager : MonoBehaviour
{
    [SerializeField] private List<Seat> allSeats = new List<Seat>();

    public Seat GetRandomEmptySeat()
    {
        List<Seat> emptySeats = new List<Seat>();

        for(int i=0;  i< allSeats.Count; i++)
        {
            if(allSeats[i].isOccupied == false)
            {
                emptySeats.Add(allSeats[i]);
            }
        }

        if(emptySeats.Count > 0)
        {
            int randomIndex = Random.Range(0, emptySeats.Count);
            return emptySeats[randomIndex];
        }
        return null;
    }
}
