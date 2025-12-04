using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
public class Positioner : MonoBehaviour
{
    public Transform m_positionA;
    public Transform m_positionB;

    public PositionUnion[] m_position_union_list;

    public Func<Vector3,Vector3 , int, int, bool> h_is_connect;
    public virtual Vector3 get_max_position
    {
        get
        {
            float x_max = Mathf.Max(m_positionA.position.x, m_positionB.position.x);
            float y_max = Mathf.Max(m_positionA.position.y, m_positionB.position.y);
            float z_max = Mathf.Max(m_positionA.position.z, m_positionB.position.z);
            return new Vector3(x_max, y_max, z_max);
        }
    }
    public virtual Vector3 get_min_position
    {
        get
        {
            float x_min = Mathf.Min(m_positionA.position.x, m_positionB.position.x);
            float y_min = Mathf.Min(m_positionA.position.y, m_positionB.position.y);
            float z_min = Mathf.Min(m_positionA.position.z, m_positionB.position.z);
            return new Vector3(x_min, y_min, z_min);
        }
    }
    public virtual Vector3[] h_create_position_list()
    {
        throw new NotImplementedException();
    }
    public virtual PositionUnion[] h_create_union_list()
    {
        throw new NotImplementedException();
    }
}
