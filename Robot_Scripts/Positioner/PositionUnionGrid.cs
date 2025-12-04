using UnityEngine;

public class PositionUnionGrid : PositionUnion
{
    public int m_length_x;
    public int m_length_y;

    public int m_offset_x;
    public int m_offset_y;

    // 생성자: 1차원 nullable 벡터 배열
    public PositionUnionGrid(Vector3?[] position, int lengthX, int lengthY, int offsetX = 0, int offsetY = 0)
        : base(System.Array.ConvertAll(position, p => p ?? Vector3.zero)) // base는 Vector3[]이므로 null 방지
    {
        m_length_x = lengthX;
        m_length_y = lengthY;
        m_offset_x = offsetX;
        m_offset_y = offsetY;
    }

    // 생성자: 2차원 nullable 배열을 받아 1차원으로 평탄화
    public PositionUnionGrid(Vector3?[,] position)
        : base(Flatten(position)) // base 생성자 호출 추가
    {
        m_length_x = position.GetLength(0);
        m_length_y = position.GetLength(1);
        m_offset_x = 0;
        m_offset_y = 0;
    }

    private static Vector3?[] Flatten(Vector3?[,] input)
    {
        int width = input.GetLength(0);
        int height = input.GetLength(1);
        Vector3?[] flat = new Vector3?[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                flat[y * width + x] = input[x, y] ?? Vector3.zero; // null이면 zero 대입
            }
        }
        return flat;
    }

    public Vector3? get_position(int x, int y)
    {
        int index = (y - m_offset_y) * m_length_x + (x - m_offset_x);
        if (index < 0 || index >= m_position_list.Length) return null;
        Vector3 value = m_position_list[index].Value;
        return value == Vector3.zero ? (Vector3?)null : value;
    }
    public Vector3? get_position((int,int) xy)
    {
        return get_position(xy.Item1, xy.Item2);
    }
    public void set_position(int x, int y, Vector3? value)
    {
        int index = (y - m_offset_y) * m_length_x + (x - m_offset_x);
        if (index < 0 || index >= m_position_list.Length) return;
        m_position_list[index] = value ?? Vector3.zero;
    }
    public void set_position((int,int )xy , Vector3? value)
    {
        set_position(xy.Item1, xy.Item2, value);
    }
}
