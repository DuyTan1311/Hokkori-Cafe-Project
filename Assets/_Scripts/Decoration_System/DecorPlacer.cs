using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class DecorPlacer : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask surfaceLayerMask;
    [SerializeField] private float rayDistance = 100f;
    [SerializeField] private DecorPlacedEvent onDecorPlaced;
    [SerializeField] private DecorRemovedEvent onDecorRemoved;

    private DecorGhost _ghost;
    private DecorData _pendingData;
    private bool _isPlacing;

    public void BeginPlacing(DecorData data)
    {
        _pendingData = data;
        _isPlacing = true;
        SpawnGhost(data);
    }

    public void CancelPlacing()
    {
        _isPlacing = false;
        if(_ghost != null)
        {
            Destroy(_ghost.gameObject);
        }
    }

    public void OnPlaceInput()
    {
        if(!_isPlacing || _ghost == null || !_ghost.CanPlace)
        {
            return;
        }
        Place();
    }

    public void OnRotateInput()
    {
        if(_isPlacing && _ghost != null)
        {
            _ghost.Rotate();
        }
    }

    public void OnCancelInput()
    {
        CancelPlacing();
    }

    private void Update()
    {
        if(!_isPlacing || _ghost == null)
        {
            return;
        }
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue()), out RaycastHit hit, rayDistance, surfaceLayerMask))
        {
            if(hit.collider.TryGetComponent(out IGridSurface surface))
            {
                _ghost.UpdateTarget(hit.point, surface);
            }
        }
    }

    private void Place()
    {
        IGridSurface surface = _ghost.CurrentSurface;
        Vector3Int origin = _ghost.SnappedCell;
        float snapRot = _ghost.SnappedRotationY;

        var cells = DecorGhost.GetOccupiedCells(origin, _pendingData.footprintSize, _pendingData.pivotOffset, snapRot);

        Vector3 worldPos = surface.CellToWorld(origin);
        var instance = Instantiate(_pendingData.prefab, worldPos, Quaternion.Euler(0f, snapRot, 0f));
        var obj = instance.GetComponent<DecorObject>();
        obj.Init(_pendingData);

        var placementData = new DecorPlacementData
        {
            decorData = _pendingData,
            decorObject = obj,
            originCell = origin,
            occupiedCells = cells,
            rotation = snapRot
        };

        surface.RegisterCells(cells, placementData);
        obj.SetPlacementData(placementData);

        if (_pendingData.isStackableSurface)
        {
            obj.EnableStackableSurface();
        }
        
        onDecorPlaced?.Raise(placementData);
        CancelPlacing();
    }

    private void SpawnGhost(DecorData data)
    {
        if(_ghost != null)
        {
            Destroy(_ghost.gameObject);
        }
        var go = Instantiate(data.prefab);
        _ghost = go.AddComponent<DecorGhost>();
        _ghost.Init(data);
        foreach(var col in go.GetComponentsInChildren<Collider>())
        {
            col.enabled = false;
        }
    }
}
