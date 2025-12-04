using UnityEngine;

public struct RayLayer 
{
    public Vector3 m_position0;
    public Vector3 m_position1;
    public Vector3 m_position2;
    public Vector3 m_position3;

    public Vector3 m_direction;

    public int m_resolution_x;
    public int m_resolution_z;

    public Vector3?[,] m_ray_hit_position_list;

    public Vector3 get_vector_x
    {
        get
        {
            return m_position1 - m_position0;
        }
    }
    public Vector3 get_vector_z
    {
        get
        {
            return m_position3 - m_position0;
        }
    }
    public RayLayer(Vector3 position0, Vector3 position1, Vector3 position2 , Vector3 position3, Vector3 direction, float height, int resolution_x, int resolution_z)
    {
        m_position0 = position0 + direction *height;
        m_position1 = position1+ direction *height;
        m_position2 = position2 + direction * height;
        m_position3 = position3 + direction * height;
        m_direction = direction;
        m_resolution_x = resolution_x;
        m_resolution_z = resolution_z;
        m_ray_hit_position_list = null;
    }
    public RayLayer(Vector3 position0, Vector3 position1, Vector3 position2 , Vector3 position3, Vector3 direction, int resolution_x, int resolution_z)
    {
        m_position0 = position0;
        m_position1 = position1;
        m_position2 = position2;
        m_position3 = position3;
        m_direction = direction;
        m_resolution_x = resolution_x;
        m_resolution_z = resolution_z;
        m_ray_hit_position_list = null;
    }
    public Vector3[,] get_resolution_position
    {
        get
        {
            if (m_resolution_x <= 2 && m_resolution_z <= 2)
                return new Vector3[2,2]
                {
                    { m_position0,
                    m_position1 },
                    { m_position2,
                    m_position3 }
                };
            int resolution_x = m_resolution_x;
            int resolution_z = m_resolution_z;
            if (resolution_x <= 2) resolution_x = 2;
            if (resolution_z <= 2) resolution_z = 2;
            Vector3[,] result = new Vector3[resolution_x,resolution_z];
            for(int idx_x = 0; idx_x< resolution_x; idx_x++)
            {
                for(int idx_z = 0; idx_z< resolution_z; idx_z++)
                {
                    float per_x = (float)idx_x / (resolution_x - 1);
                    float per_z = (float)idx_z / (resolution_z - 1);
                    Vector3 position = m_position0 + get_vector_x * per_x + get_vector_z * per_z;
                    result[idx_x, idx_z] = position;
                }
            }
            return result;
        }
    }
    public Vector3?[,] get_ray_hit_position
    {
        get
        {
            Vector3[,] resolution_position = get_resolution_position;
            int length_x = resolution_position.GetLength(0);
            int length_z = resolution_position.GetLength(1);
            Vector3?[,] result = new Vector3?[length_x, length_z ];
            for(int idx_x =0; idx_x<length_x; idx_x++)
            {
                for (int idx_z = 0; idx_z < length_z; idx_z++)
                {
                    result[idx_x,idx_z] = null;
                    Vector3 from = resolution_position[idx_x, idx_z];
                    if (Physics.Raycast(new Ray(from, m_direction), out RaycastHit hit))
                    {
                        result[idx_x, idx_z] = hit.point;
                    }
                }
            }
            return result;
        }
    }
}
