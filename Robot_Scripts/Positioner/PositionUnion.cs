using UnityEngine;
using System.Collections.Generic;

public class PositionUnion
{
    public Vector3?[] m_position_list;

    // Vector3[] → Vector3?[]로 변환
    public PositionUnion(Vector3[] position)
    {
        m_position_list = System.Array.ConvertAll(position, p => (Vector3?)p);
    }
    public PositionUnion(Vector3?[] position)
    {
        m_position_list = position;
    }

    // List<Vector3> → Vector3?[]로 변환
    public PositionUnion(List<Vector3> position)
    {
        m_position_list = position.ConvertAll(p => (Vector3?)p).ToArray();
    }
    public PositionUnion(List<Vector3?> position)
    {
        m_position_list = position.ToArray();
    }
}