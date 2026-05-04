using System.Collections.Generic;
using UnityEngine;

public interface IGridSurface
{
    DecorSurface SurfaceType {get; }
    float CellSize{get; }
    Vector3Int WorldToCell(Vector3 worldPos);
    Vector3 CellToWorld(Vector3Int cell);
    bool CanPlace(List<Vector3Int> cells);
    void RegisterCells(List<Vector3Int> cells, DecorPlacementData data);
    void UnregisterCells(List<Vector3Int> cells);
    bool TryGetPlacement(Vector3Int cell, out DecorPlacementData data);
}
