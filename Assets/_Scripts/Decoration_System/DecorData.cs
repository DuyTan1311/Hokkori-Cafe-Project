using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Decor/New Decor Data")]
public class DecorData : ScriptableObject
{
    public string itemName;
    public string poolKey;
    public GameObject prefab;
    public Vector2Int footprintSize;
    public Vector2Int pivotOffset; // khoảng cách hay độ lệch (offset) tính từ origin cell (cell gốc) đến pivot của object
    public DecorSurface requiredSurface;
    public bool isStackableSurface; // có thể đặt đồ lên
    public Vector2 stackableSurfaceSize; // nếu isStackableSurface true
}
