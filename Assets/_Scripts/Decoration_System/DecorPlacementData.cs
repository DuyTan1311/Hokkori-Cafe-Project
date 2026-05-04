using System.Collections.Generic;
using UnityEngine;

public class DecorPlacementData
{
   public DecorData decorData;
   public DecorObject decorObject;
   public Vector3Int originCell;
   public List<Vector3Int> occupiedCells;
   public float rotation; // góc Y sau khi snap
}
