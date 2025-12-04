using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Perception.Randomization.Utilities;

public static class YCB_AreaFunction
{
    public static int sampling_max = 1000;
    public static float interRange_max = 1;
    public static float interRange_min = 0.01f;
    public static float interRange_current;

    public static List<Vector3> sampling_grid(IYCB_Area ycbarea)
    {
        get_interRangeReset();
        for (int i = 0; i < sampling_max; i++)
        {
            float inter = get_interRange();
            List<Vector3> result = new List<Vector3>();
            Vector3 min = ycbarea.samplingAreaMin();
            Vector3 max = ycbarea.samplingAreaMax();

            Vector3 size = max - min;

            // offset은 itter 이하에서만 랜덤, 넘어가지 않도록 clamp
            float offsetX = Mathf.Min(size.x, Random.Range(0f, inter));
            float offsetY = Mathf.Min(size.y, Random.Range(0f, inter));
            float offsetZ = Mathf.Min(size.z, Random.Range(0f, inter));


            for (float x = min.x + offsetX; x <= max.x + float.Epsilon; x += inter)
            {
                for (float y = min.y + offsetY; y <= max.y + float.Epsilon; y += inter)
                {
                    for (float z = min.z + offsetZ; z <= max.z + float.Epsilon; z += inter)
                    {
                        Vector3 pos = new Vector3(x, y, z);
                        if (ycbarea.isArea(pos))
                        {
                            result.Add(pos);
                        }
                    }
                }
            }

            if (result.Count > 0)
            {
                return result;
            }
        }
        return null;
    }
    public static Vector3[] sampling_grid_arr(IYCB_Area ycbarea)
    {
        List<Vector3> grid = sampling_grid(ycbarea);
        if (grid == null) return null;

        else return grid.ToArray();
    }
    public static List<Vector3> sampling_grid(IYCB_Area ycbarea, float  inter)
    {
        List<Vector3> result = new List<Vector3>();
        if (inter <= float.Epsilon)
            return result;
        Vector3 min = ycbarea.samplingAreaMin();
        Vector3 max = ycbarea.samplingAreaMax();

        Vector3 size = max - min;
        float offsetX = Mathf.Min(size.x, Random.Range(0f, inter));
        float offsetY = Mathf.Min(size.y, Random.Range(0f, inter));
        float offsetZ = Mathf.Min(size.z, Random.Range(0f, inter));

        for (float x = min.x + offsetX; x <= max.x + float.Epsilon; x += inter)
        {
            for (float y = min.y + offsetY; y <= max.y + float.Epsilon; y += inter)
            {
                for (float z = min.z + offsetZ; z <= max.z + float.Epsilon; z += inter)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    if (ycbarea.isArea(pos))
                    {
                        result.Add(pos);
                    }
                }
            }
        }
        return result;
    }
    public static Vector3[] sampling_grid_arr(IYCB_Area ycbarea , float inter)
    {
        List<Vector3> grid = sampling_grid(ycbarea);
        if (grid == null) return null;

        else return grid.ToArray();
    }
    public static float get_interRange()
    {
        float divid = Random.Range(0.95f, 1.0f);
        interRange_current = divid * interRange_current;
        if (interRange_current < interRange_min)
            interRange_current = interRange_max;
        return interRange_current;
    }
    public static void get_interRangeReset()
    {
        interRange_current = interRange_max;
    }

}