using UnityEngine;
[CreateAssetMenu(fileName = "PlaceableObjectData", menuName = "Decor/PlaceableObjectData")]
public class PlaceableObjectData : ScriptableObject // decor không phải inventory item nên không dùng itemData
{
    public string objectName;
    public Vector2Int size = Vector2Int.one; // số ô chiếm dụng trong grid
    public SurfaceType[] allowedSurfaces; // bề mặt có thể đặt
    public GameObject prefab; // model được spawn ra
    public bool canBeStackedOn; // object khác có thể đặt lên được không

    public bool IsAllowedOn(SurfaceType type) // hàm check xem object này có đặt được lên một surface không
    {
        foreach(var s in allowedSurfaces)
        {
            if(s == type)
            {
                return true;
            }
        }
        return false;
    }
}
