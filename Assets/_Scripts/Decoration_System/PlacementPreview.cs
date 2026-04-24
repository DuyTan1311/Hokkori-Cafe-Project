using UnityEngine;

public class PlacementPreview : MonoBehaviour // class quản lý cơ chế preview của decor
{
    private static readonly int BaseColorProp = Shader.PropertyToID("_BaseColor"); // lấy base color từ material của object

    [SerializeField] private Color validColor = new Color(0f, 1f, 0f, 0.4f); // màu xanh valid
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.4f); // màu đỏ invalid

    private GameObject ghostObject; // object preview
    private Renderer[] ghostRenderers; // tất cả renderer của object vì 1 prefab có thể gồm nhiều renderer cho từng bộ phận
    private MaterialPropertyBlock propertyBlock; // property block để chỉnh sửa material mà không cần tạo thêm nhiều material instance

    public void Show(PlaceableObjectData data) // hàm để show preview object khi player decor
    {
        if(ghostObject != null)
        {
            Hide(); // nếu ghost object vẫn còn thì xóa bỏ (hide)
        }
        ghostObject = Instantiate(data.prefab); // tạo instance prefab mới

        foreach(var col in ghostObject.GetComponentsInChildren<Collider>()) // bỏ hết collider để tránh rối raycast vì raycast cần đi xuyên qua ghost object
        {
            col.enabled = false;
        }
        ghostRenderers = ghostObject.GetComponentsInChildren<Renderer>(); // lấy các renderer ra
        propertyBlock = new MaterialPropertyBlock(); // tạo material property block mới
    }

    public void Hide() // hàm dùng để xóa bỏ ghost object
    {
        if(ghostObject != null)
        {
            Destroy(ghostObject); // nếu ghost object không null thì destroy
        }
        ghostObject = null; // set ghost object về null
    }

    public void UpdatePose(Vector3 worldPos, Quaternion rotation, bool isValid) // hàm chạy mỗi frame để update pose của object decor, được gọi bởi class khác
    {
        if(ghostObject == null) // nếu ghost object bằng null thì dừng
        {
            return;
        }

        ghostObject.transform.SetPositionAndRotation(worldPos, rotation); // set transform và cập nhật mỗi frame
        Color tint = isValid ? validColor : invalidColor; // set màu tint nếu valid hay không
        foreach(var r in ghostRenderers) // duyệt qua các renderer và set tint color
        {
            r.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor(BaseColorProp, tint);
            r.SetPropertyBlock(propertyBlock);
        }
    }

    public bool IsActive => ghostObject != null; // hàm check ghost object có active hay không
}
