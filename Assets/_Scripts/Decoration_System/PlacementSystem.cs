using UnityEngine;

[RequireComponent(typeof(PlacementPreview))]
public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private Camera placementCamera; // camera để bắn ray
    [SerializeField] private LayerMask surfaceLayer; // layer để của surface để bắn ray tới
    [SerializeField] private float raycastDistance = 100f; // khoảng cách raycast

    private PlacementPreview preview; // preview object
    private PlaceableObjectData selectedData; // object decor được chọn
    private PlacementSurface currentSurface; // bề mặt đang hover chuột lên
    private Vector2Int currentCell; // ô grid mà chuột đang trỏ vào
    private int rotationStep; // từ 0-3, mỗi step bằng 90 độ

    private Vector2Int CurrentSize // số ô chiếm của object
    {
        get
        {
            if(rotationStep % 2 == 0) // nếu ở bước 0 hoặc 2 (0 độ hoặc 180 độ) thì giữ nguyên
            {
                return selectedData.size;
            }
            else // nếu ở bước 1 hoặc 3 (90 hoặc 270 độ) thì đổi x với y lại
            {
                return new Vector2Int(selectedData.size.y, selectedData.size.x);
            }
        }
    }

    private Quaternion CurrentRotation // rotation quanh trục y
    {
        get
        {
            float angle = rotationStep * 90f;
            return Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void Awake()
    {
        preview = GetComponent<PlacementPreview>(); // lấy placment preview
    }

    // public API để gọi từ bên ngoài (input handler hoặc test script)

    public void BeginPlacement(PlaceableObjectData data) // hàm gọi khi player click vào item decor
    {
        selectedData = data; // lưu item
        rotationStep = 0; // reset rotation
        preview.Show(data); // show preview
    }

    public void Rotate() // hàm gọi khi player xoay object
    {
        if(selectedData == null) // nếu item null thì dừng
        {
            return;
        }
        rotationStep = (rotationStep + 1) % 4;
        // xoay ở bước 0: (0 + 1) % 4 = 1
        // xoay ở bước 1: (1 + 1) % 4 = 2
        // xoay ở bước 2: (2 + 1) % 4 = 3
        // xoay ở bước 3: (3 + 1) % 4 = 0
        
        // cách khác nếu cách kia khó hiểu
        // rotationStep = rotationStep + 1;

        // if (rotationStep >= 4)
        // {
        //     rotationStep = 0;
        // }
    }

    public void ConfirmPlacement()
    {
        if(selectedData == null || currentSurface == null)
        {
            return;
        }

        if(!currentSurface.Occupancy.CanPlace(currentCell, CurrentSize)) // check xem object có bị out of bound hay ô đó bị chiếm chưa
        {
            return;
        }

        currentSurface.Occupancy.Occupy(currentCell, CurrentSize); // đánh dấu trên grid
        Vector3 spawnPos = currentSurface.CellToWorld(currentCell); // chuyển từ tọa độ trên grid về world space để làm vị trí spawn

        spawnPos = GetPlacementCenter(currentSurface, currentCell, CurrentSize); // tính toán vị trí center của vị trí các ô chiếm để spawn

        Instantiate(selectedData.prefab, spawnPos, CurrentRotation); // tạo instance tại spawn pos

        CancelPlacement(); // thoát ra khỏi placement mode
    }

    public void CancelPlacement() // hàm thoát ra khỏi placement mode, reset tất cả về null và 0
    {
        selectedData = null;
        currentSurface = null;
        rotationStep = 0;
        preview.Hide();
    }

    // update loop

    public void Tick() // hàm chạy mỗi frame để cập nhật preview liên tục
    {
        if(selectedData == null) // nếu item null thì dừng
        {
            return;
        }

        Ray ray = placementCamera.ScreenPointToRay(GetMouseScreenPos()); // bắn ray tới vị trí chuột

        if(!Physics.Raycast(ray, out RaycastHit hit, raycastDistance, surfaceLayer)) // nếu không hit thì cho preview đỏ và dừng hàm
        {
            preview.UpdatePose(Vector3.zero, CurrentRotation, false);
            return;
        }

        var surface = hit.collider.GetComponentInParent<PlacementSurface>(); // lấy placement surface
        if(surface == null || !selectedData.IsAllowedOn(surface.surfaceType)) // nếu surface null hoặc surface không hợp lệ thì set preview đỏ và dừng hàm
        {
            preview.UpdatePose(hit.point, CurrentRotation, false);
            return;
        }

        currentSurface = surface; // set current surface
        currentCell = surface.WorldToCell(hit.point); // chuyển hit point sang tọa độ grid
        bool isValid = surface.Occupancy.CanPlace(currentCell, CurrentSize); // check xem có thể đặt trên surface không
        Vector3 snappedPos = GetPlacementCenter(surface, currentCell, CurrentSize); // lấy vị trí snap ở trung tâm grid
        preview.UpdatePose(snappedPos, CurrentRotation, isValid); // update pose và set preview xanh
    }

    private Vector3 GetPlacementCenter(PlacementSurface surface, Vector2Int origin, Vector2Int size)
    {
        Vector3 originWorld = surface.CellToWorld(origin); // lấy điểm góc dưới bên trái
        Vector3 farCornerWorld = surface.CellToWorld(origin + size - Vector2Int.one); // lấy điểm góc trên bên phải
        return (originWorld + farCornerWorld) * 0.5f; // tính trung bình để ra center
    }

    private Vector3 GetMouseScreenPos() // lấy mouse position dựa trên input system
    {
#if ENABLE_INPUT_SYSTEM
        return UnityEngine.InputSystem.Mouse.current.position.ReadValue();
#else
        return Input.mousePosition;
#endif   
    }
}

// luồng logic:
// BeginPlacement()
//     ↓
// Tick() mỗi frame
//     ↓
// Raycast → Surface
//     ↓
// World → Cell
//     ↓
// CanPlace?
//     ↓
// Preview (xanh/đỏ)
//     ↓
// Click
//     ↓
// ConfirmPlacement()
//     ↓
// Occupy grid
//     ↓
// Spawn object thật
