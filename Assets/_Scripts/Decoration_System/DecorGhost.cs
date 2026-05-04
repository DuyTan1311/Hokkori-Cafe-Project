using UnityEngine;
using System.Collections.Generic;

public class DecorGhost : MonoBehaviour
{
    [Header("Lerp Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float rotateSpeed = 12f;
    [SerializeField] private float smoothTime = 0.08f;

    private DecorData _decorData;
    private IGridSurface _currentSurface;

    // --- Dual-transform state ---
    private Vector3 _visualPosition;
    private Vector3 _posVelocity;
    private float _visualRotationY;
    private float _rotVelocity;
    private float _targetRotationY;     // tích lũy góc, luôn là bội số 90
    private Vector3Int _snappedCell;    // cell grid thực sự
    private bool _canPlace;

    private MeshRenderer[] _renderers;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");

    public Vector3Int SnappedCell => _snappedCell;
    public float SnappedRotationY => Mathf.Round(_targetRotationY / 90f) * 90f;
    public bool CanPlace => _canPlace;
    public IGridSurface CurrentSurface => _currentSurface;

    public void Init(DecorData data)
    {
        _decorData = data;
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _visualPosition = transform.position;
        _visualRotationY = 0f;
        _targetRotationY = 0f;
    }

    // Gọi từ DecorPlacer mỗi frame khi có hit
    public void UpdateTarget(Vector3 hitPoint, IGridSurface surface)
    {
        _currentSurface = surface;
        _snappedCell = surface.WorldToCell(hitPoint);
        // snap về center ô grid, nhưng chỉ dùng cho logic — visual lerp tới đây
        Vector3 snappedWorld = surface.CellToWorld(_snappedCell);
        _visualPosition = Vector3.SmoothDamp(_visualPosition, snappedWorld, ref _posVelocity, smoothTime);

        var occupied = GetOccupiedCells(_snappedCell, _decorData.footprintSize, _decorData.pivotOffset, SnappedRotationY);
        _canPlace = surface.SurfaceType == _decorData.requiredSurface && surface.CanPlace(occupied);

        UpdateVisual();
    }

    public void Rotate()
    {
        _targetRotationY += 90f;
    }

    private void Update()
    {
        // smooth rotation độc lập với UpdateTarget
        _visualRotationY = Mathf.SmoothDampAngle(_visualRotationY, _targetRotationY, ref _rotVelocity, smoothTime);
        transform.SetPositionAndRotation(
            _visualPosition,
            Quaternion.Euler(0f, _visualRotationY, 0f)
        );
    }

    private void UpdateVisual()
    {
        Color tint = _canPlace ? new Color(0f, 1f, 0f, 0.4f) : new Color(1f, 0f, 0f, 0.4f);
        foreach (var r in _renderers)
        {
            r.material.color = tint;
            r.material.SetColor(EmissionColor, tint * 0.6f);
        }
    }

    // Tính list ô bị chiếm dựa trên footprint + góc xoay snap
    public static List<Vector3Int> GetOccupiedCells(Vector3Int pivotCell, Vector2Int size, Vector2Int pivot, float snapRotY)
    {
        var cells = new List<Vector3Int>();
        int rot = Mathf.RoundToInt(snapRotY / 90f) % 4;
        if (rot < 0) rot += 4;

        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                int dx = x - pivot.x;
                int dz = z - pivot.y;
                Vector3Int offset = rot switch
                {
                    1 => new Vector3Int(dz, 0, -dx),
                    2 => new Vector3Int(-dx, 0, -dz),
                    3 => new Vector3Int(-dz, 0, dx),
                    _ => new Vector3Int(dx, 0, dz)
                };
                cells.Add(pivotCell + offset);
            }
        }
        return cells;
    }

    private void OnDrawGizmos()
    {
        if (_decorData == null || _currentSurface == null) return;

        var cells = GetOccupiedCells(_snappedCell, _decorData.footprintSize, _decorData.pivotOffset, SnappedRotationY);
        Gizmos.color = _canPlace ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);
        float cs = _currentSurface.CellSize;
        foreach (var cell in cells)
        {
            Vector3 center = _currentSurface.CellToWorld(cell);
            Gizmos.DrawWireCube(center + Vector3.up * 0.05f, new Vector3(cs * 0.9f, 0.1f, cs * 0.9f));
        }
    }
}
