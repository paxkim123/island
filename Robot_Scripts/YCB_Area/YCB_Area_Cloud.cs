using JetBrains.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class YCB_Area_Cloud : MonoBehaviour
{
    public float gab;


    public float per;

    public Transform locateA;
    public Transform locateB;

    public YCB_Area ycb_area;
    public List<ACloud> cloud_list = new List<ACloud>();
    public Color color_gizmos;
    public bool is_gizmos;

    public void CreateCloud()
    {
        cloud_list = new List<ACloud>();
        Vector3 max = new Vector3(
            Mathf.Max(locateA.position.x, locateB.position.x),
            Mathf.Max(locateA.position.y, locateB.position.y),
            Mathf.Max(locateA.position.z, locateB.position.z)
            );
        Vector3 min = new Vector3(
            Mathf.Min(locateA.position.x, locateB.position.x),
            Mathf.Min(locateA.position.y, locateB.position.y),
            Mathf.Min(locateA.position.z, locateB.position.z)
            );
        IYCB_Area iya = ycb_area as IYCB_Area;
        if (iya != null)
        {
            for (float i = min.x; i < max.x; i += gab)
                for (float j = min.y; i < max.y; j += gab)
                    for (float k = min.z; i < max.z; k += gab)
                    {
                        bool isArea = iya.isArea(new Vector3(i, j, k));
                        cloud_list.Add(new ACloud(new Vector3(i, j, k), isArea));
                        Debug.Log($"{new Vector3(i,j,k).ToString()} Point Create");
                    }
        }
        else
        {
            Debug.Log("Area is null");
        }
    }
    public void CreateCloudParallel()
    {
        cloud_list = new List<ACloud>();
        Vector3 max = new Vector3(
           Mathf.Max(locateA.position.x, locateB.position.x),
           Mathf.Max(locateA.position.y, locateB.position.y),
           Mathf.Max(locateA.position.z, locateB.position.z)
           );
        Vector3 min = new Vector3(
            Mathf.Min(locateA.position.x, locateB.position.x),
            Mathf.Min(locateA.position.y, locateB.position.y),
            Mathf.Min(locateA.position.z, locateB.position.z)
            );

        IYCB_Area iya = ycb_area as IYCB_Area;
        if (iya == null)
        {
            Debug.Log("Area is null");
            return;
        }

        List<ACloud> tempList = new List<ACloud>(); // temp list
        object _lock = new object();

        int stepsX = Mathf.CeilToInt((max.x - min.x) / gab);
        int stepsY = Mathf.CeilToInt((max.y - min.y) / gab);
        int stepsZ = Mathf.CeilToInt((max.z - min.z) / gab);

        Parallel.For(0, stepsX, ix =>
        {
            for (int iy = 0; iy < stepsY; iy++)
            {
                for (int iz = 0; iz < stepsZ; iz++)
                {
                    Vector3 pos = new Vector3(
                        min.x + ix * gab,
                        min.y + iy * gab,
                        min.z + iz * gab
                    );
                    bool isArea = iya.isArea(pos); // be careful!
                    ACloud cloud = new ACloud(pos, isArea);
                    lock (_lock)
                    {
                        tempList.Add(cloud); // thread-safe add
                    }
                }
            }
        });

        cloud_list = tempList;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = color_gizmos;
        foreach (ACloud e in cloud_list)
        {
            Gizmos.DrawCube(e.position, Vector3.one);
        }    
    }
    public struct ACloud
    {
        public bool isArea;
        public Vector3 position;
        public ACloud(Vector3 _position, bool _isArea)
        {
            position = _position;
            isArea = _isArea;
        }
    }
}
