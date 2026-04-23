using UnityEngine;

public class PlacementSurface : MonoBehaviour // class này quyết định logic của bề mặt có thể đặt lên
{
    [SerializeField] public SurfaceType surfaceType; // loại surface
    [SerializeField] public int gridWidth = 10; // kích thước grid
    [SerializeField] public int gridHeight = 10;
    [SerializeField] public float cellSize = 1f; // kích thước ô grid

    [SerializeField] public Vector3 localOrigin = Vector3.zero; // gốc của grid ở local space, nằm ở góc trái dưới

    public GridOccupancy Occupancy {get; private set; } // grid của bề mặt

    private void Awake()
    {
        Occupancy = new GridOccupancy(gridWidth, gridHeight); // tạo grid mới
    }

    public Vector2Int WorldToCell(Vector3 worldPos) // hàm chuyển tọa độ từ world space sang grid space
    {
        Vector3 localPos = transform.InverseTransformPoint(worldPos) - localOrigin; // chuyển tọa độ world sang local
        int x = Mathf.FloorToInt(localPos.x / cellSize); // chuyển sang tọa độ x của grid
        int y;
        if(surfaceType == SurfaceType.Wall)
        {
            y = Mathf.FloorToInt(localPos.y / cellSize); // nếu là tường thì dùng y local chuyển sang tọa độ y của grid
        }
        else
        {
            y = Mathf.FloorToInt(localPos.z / cellSize); // nếu không phải tường thì lấy z local chuyển sang tọa độ y của grid
        }

        return new Vector2Int(x, y); // trả về tạo độ 2 chiều trên grid
    }

    public Vector3 CellToWorld(Vector2Int cell) // hàm chuyển từ tọa độ grid sang world space
    {
        float lx = (cell.x + 0.5f) * cellSize + localOrigin.x; // chuyển x từ grid sang local space
        float ly, lz;

        if(surfaceType == SurfaceType.Wall) // nếu là tường thì chuyển y từ grid sang y ở local space, z giữ nguyên
        {
            ly = (cell.y + 0.5f) * cellSize + localOrigin.y; 
            lz = localOrigin.z;
        }
        else // nếu không phải tường thì giữ nguyên y, chuyển z từ grid sang y ở local space
        {
            ly = localOrigin.y;
            lz = (cell.y + 0.5f) * cellSize + localOrigin.z;
        }

        return transform.TransformPoint(new Vector3(lx, ly, lz));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for(int x = 0; x < gridWidth; x++)
        for(int y = 0; y < gridHeight; y++)
            {
                Vector3 center = CellToWorld(new Vector2Int(x, y));
                Gizmos.DrawWireCube(center, Vector3.one * cellSize * 0.95f);
            }
    }
}
