using UnityEngine;
using System.Collections.Generic;
using System;
public class PSRayLayer : Positioner
{
    public int m_resolution_x;
    public int m_resolution_z;
    public int m_resolution_height;

    public RayLayer[] m_raylayer_list;
    public bool m_flow;
    public float m_connect_gradient = 2f;
    public Vector3 get_base_direction_ray
    {
        get
        {
            return Vector3.down;
        }
    }
    public Vector3 get_base_direction_x
    { get { return Vector3.left; } }
    public Vector3 get_base_direction_z
    { get { return Vector3.back; } }
    public Vector3 get_direction_ray
    { get { return (this.transform.rotation * get_base_direction_ray).normalized; } }
    public Vector3 get_direction_x
    { get { return (this.transform.rotation * get_base_direction_x).normalized; } }
    public Vector3 get_direction_z
    { get { return (this.transform.rotation * get_base_direction_z).normalized; } }

    public float get_height
    {
        get
        {
            Vector3 getDirectionRay = get_direction_ray;
            float dotA = Vector3.Dot(m_positionA.position, getDirectionRay);
            float dotB = Vector3.Dot(m_positionB.position, getDirectionRay);
            return Mathf.Abs(dotA - dotB);
        }
    }
    public RayLayer get_raylayer(float per)
    {
        Vector3 max = get_max_position;
        Vector3 min = get_min_position;
        Vector3 ray = get_direction_ray;
        Vector3 getDirectionX = get_base_direction_x;
        Vector3 getDirectionZ = get_base_direction_z;

        Vector3 pmax = max - Vector3.Dot(max - min, ray) * ray;
        Vector3 diff = pmax - min;
        Vector3 XLocate = min + Vector3.Dot(diff, getDirectionX) * getDirectionX;
        Vector3 ZLocate = min + Vector3.Dot(diff, getDirectionZ) * getDirectionZ;
        float height = per * get_height;
        return new RayLayer(min, XLocate, pmax, ZLocate, ray, height, m_resolution_x, m_resolution_z);
    }
    public RayLayer[] h_create_raylayer_list()
    {
        List<RayLayer> result = new List<RayLayer>();
        int resolution_height = m_resolution_height;
        if (resolution_height < 2) resolution_height = 2;
        for (int i = 0; i < resolution_height; i++)
        {
            float per = (float)i / (float)(resolution_height - 1);
            result.Add(get_raylayer(per));
        }
        m_raylayer_list = result.ToArray();
        return m_raylayer_list;
    }
    public override Vector3[] h_create_position_list()
    {
        m_raylayer_list = h_create_raylayer_list();
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < m_raylayer_list.Length; i++)
        {
            Vector3?[,] resolution_position = m_raylayer_list[i].get_ray_hit_position;
            for (int idx_x=0; idx_x< resolution_position.GetLength(0); idx_x++)
            {
                for (int idx_z=0; idx_z <resolution_position.GetLength(1); idx_z++)
                {
                    if(resolution_position[idx_x,idx_z]!=null)
                    {
                        result.Add(resolution_position[idx_x, idx_z].Value);
                    }
                }
            }   
        }
        return result.ToArray();
    }
    public override PositionUnion[] h_create_union_list()
    {
        m_raylayer_list = h_create_raylayer_list();
        for (int i = 0; i< m_raylayer_list.Length; i++)
        {
            Vector3?[,] grid = m_raylayer_list[i].get_ray_hit_position;
            int[,] grid_parents = new int[grid.GetLength(0), grid.GetLength(1)];
            for (int idx_x = 0; idx_x < grid.GetLength(0); idx_x++)
                for (int idx_z = 0; idx_z < grid.GetLength(1); idx_z++)
                {
                    if (grid[idx_x, idx_z] != null)
                    {
                        grid_parents[idx_x, idx_z] = i;
                    }
                }
        }

        throw new NotImplementedException();
    }
}

