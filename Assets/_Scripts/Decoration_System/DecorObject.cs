using UnityEngine;

public class DecorObject : MonoBehaviour
{
    private DecorData _data;
    private DecorPlacementData _placementData;
    private GridSurface _stackableSurface;

    public void Init(DecorData data)
    {
        _data = data;
    }

    public void SetPlacementData(DecorPlacementData placementData)
    {
        _placementData = placementData;
    }

    public DecorPlacementData GetPlacementData() => _placementData;

    public void EnableStackableSurface()
    {
        _stackableSurface = GetComponentInChildren<GridSurface>(includeInactive: true);
        if(_stackableSurface != null)
        {
            _stackableSurface.gameObject.SetActive(true);
        }
    }
}
