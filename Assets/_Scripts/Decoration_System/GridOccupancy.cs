using UnityEngine;

public class GridOccupancy //quản lý ô nào bị chiếm trên surface
{
    private bool [,] cells; // mảng 2 chiều chứa ô grid và giá trị bool mỗi ô để biết ô nào đã bị chiếm
    public int Width {get; }
    public int Height {get; }

    public GridOccupancy(int width, int height)
    {
        Width = width;
        Height = height;
        cells = new bool[width, height];
    }

    public bool IsInBounds(int x, int y) =>
        x >= 0 && y >= 0 && x < Width && y < Height; // kiểm tra xem tọa độ có nằm trong grid không

    public bool CanPlace(Vector2Int origin, Vector2Int size) // hàm kiểm tra xem object có đặt được không, origin là góc dưới bên trái object, size là kích cỡ
    {
        for(int dx = 0; dx < size.x; dx++)
        for(int dy = 0; dy < size.y; dy++) // duyệt qua từng ô
            {
                int cx = origin.x + dx;
                int cy = origin.y + dy; // tính tọa độ thực
                if(!IsInBounds(cx, cy) || cells[cx, cy]) // nếu tọa độ nằm ngoài grid hoặc ô đó đã bị chiếm thì trả về false
                {
                    return false;
                }
            }
        return true;
    }

    public void Occupy(Vector2Int origin, Vector2Int size) // hàm đặt vào grid
    {
        for(int dx = 0; dx < size.x; dx++)
        for(int dy = 0; dy < size.y; dy++)
            {
                cells[origin.x + dx, origin.y + dy] = true; //cho tất cả ô object chiếm là true
            }
    }

    public void Release(Vector2Int origin, Vector2Int size) // hàm bỏ object khỏi grid
    {
        for(int dx = 0; dx < size.x; dx++)
        for(int dy = 0; dy < size.y; dy++)
            {
                cells[origin.x + dx, origin.y + dy] = false; //cho tất cả ô object đang chiếm trở về false
            }
    }
}
