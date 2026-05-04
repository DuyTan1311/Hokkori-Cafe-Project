using System.Collections.Generic;
using UnityEngine;

public class GridSurface : MonoBehaviour, IGridSurface
{
    [SerializeField] private DecorSurface surfaceType;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private Vector2Int gizmoGridSize = new Vector2Int(10, 10);
    [SerializeField] private DecorRemovedEvent onDecorRemoved;

    private Dictionary<Vector3Int, DecorPlacementData> occupiedCells = new();

    public DecorSurface SurfaceType => surfaceType;
    public float CellSize => cellSize;

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        Vector3 local = transform.InverseTransformPoint(worldPos);
        return new Vector3Int(
            Mathf.FloorToInt(local.x / cellSize),
            Mathf.FloorToInt(local.y / cellSize),
            Mathf.FloorToInt(local.z / cellSize)
        );
    }

    public Vector3 CellToWorld(Vector3Int cell)
    {
        Vector3 local = new Vector3(
            (cell.x + 0.5f) * cellSize,
            cell.y * cellSize,
            (cell.z + 0.5f) * cellSize
        );
        return transform.TransformPoint(local);
    }

    public bool CanPlace(List<Vector3Int> cells)
    {
        foreach(var cell in cells)
        {
            if (occupiedCells.ContainsKey(cell))
            {
                return false;
            }
        }
        return true;
    }

    public void RegisterCells(List<Vector3Int> cells, DecorPlacementData data)
    {
        foreach(var cell in cells)
        {
            occupiedCells[cell] = data;
        }
    }

    public void UnregisterCells(List<Vector3Int> cells)
    {
        foreach(var cell in cells)
        {
            occupiedCells.Remove(cell);
        }
    }

    public bool TryGetPlacement(Vector3Int cell, out DecorPlacementData data) => occupiedCells.TryGetValue(cell, out data);

    private void OnDrawGizmos()
    {
        float cs = cellSize;

        Gizmos.color = new Color(1f, 1f, 1f, 0.06f);
        for(int x=0; x < gizmoGridSize.x; x++)
        {
            for(int z = 0; z < gizmoGridSize.y; z++)
            {
                var cell = new Vector3Int(x, 0, z);
                if (occupiedCells.ContainsKey(cell))
                {
                    continue;
                }
                Vector3 center = CellToWorld(cell);
                Gizmos.DrawWireCube(center, new Vector3(cs * 0.98f, 0.02f, cs * 0.98f));
            }
        }
        Gizmos.color = new Color(0f, 1f, 0.5f, 0.15f);
        foreach (var kvp in occupiedCells)
        {
            Vector3 center = CellToWorld(kvp.Key);
            Gizmos.DrawCube(center, new Vector3(cellSize * 0.95f, 0.05f, cellSize * 0.95f));
        }
    }

    public bool RemoveDecor(Vector3Int cell)
    {
        if(!occupiedCells.TryGetValue(cell, out DecorPlacementData data))
        {
            return false;
        }

        foreach(var occupied in data.occupiedCells)
        {
            occupiedCells.Remove(occupied);
        }
        if(data.decorObject != null)
        {
            Destroy(data.decorObject.gameObject);
        }

        onDecorRemoved?.Raise(data);
        return true;
    }
}
