using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class YCB_AreaMinMax : YCB_Area, IYCB_Area
{
    public Transform locateA;
    public Transform locateB;

    public List<Vector3> samplingArea()
    {
        return YCB_AreaFunction.sampling_grid(this);
    }

    public bool isArea(Vector3 position)
    {
        float MaxX = Mathf.Max(locateA.position.x, locateB.position.x);
        float MaxY = Mathf.Max(locateA.position.y, locateB.position.y);
        float MaxZ = Mathf.Max(locateA.position.z, locateB.position.z);

        float MinX = Mathf.Min(locateA.position.x, locateB.position.x);
        float MinY = Mathf.Min(locateA.position.y, locateB.position.y);
        float MinZ = Mathf.Min(locateA.position.z, locateB.position.z);

        Vector3 vectorMax = new Vector3(MaxX, MaxY, MaxZ);
        Vector3 vectorMin = new Vector3(MinX, MinY, MinZ);
        if (position.x >= vectorMin.x && position.x <= vectorMax.x &&
       position.y >= vectorMin.y && position.y <= vectorMax.y &&
       position.z >= vectorMin.z && position.z <= vectorMax.z)
        {
            return true;
        }
        return false;
    }

    public Vector3 samplingAreaMax()
    {
        float MaxX = Mathf.Max(locateA.position.x, locateB.position.x);
        float MaxY = Mathf.Max(locateA.position.y, locateB.position.y);
        float MaxZ = Mathf.Max(locateA.position.z, locateB.position.z);
        Vector3 vectorMax = new Vector3(MaxX, MaxY, MaxZ);
        return vectorMax;
    }

    public Vector3 samplingAreaMin()
    {
        float MinX = Mathf.Min(locateA.position.x, locateB.position.x);
        float MinY = Mathf.Min(locateA.position.y, locateB.position.y);
        float MinZ = Mathf.Min(locateA.position.z, locateB.position.z);
        Vector3 vectorMin = new Vector3(MinX, MinY, MinZ);
        return vectorMin;
    }

    public bool toggleGizmos()
    {
        isGizmos = !isGizmos;
        return isGizmos;
    }

    public bool getGizmos()
    {
        return isGizmos;
    }
    void OnDrawGizmos()
    {
        // Gizmos 표시가 꺼져 있거나, locateA/B가 설정되지 않았으면 취소
        if (!isGizmos || locateA == null || locateB == null)
            return;

        // Gizmos 색상
        Gizmos.color = colorGizmos;

        // Min/Max 좌표 구하기
        Vector3 min = samplingAreaMin();
        Vector3 max = samplingAreaMax();

        // 박스 8개 꼭짓점
        Vector3 v0 = new Vector3(min.x, min.y, min.z);  // 앞-왼-아래
        Vector3 v1 = new Vector3(max.x, min.y, min.z);  // 앞-오-아래
        Vector3 v2 = new Vector3(max.x, max.y, min.z);  // 앞-오-위
        Vector3 v3 = new Vector3(min.x, max.y, min.z);  // 앞-왼-위
        Vector3 v4 = new Vector3(min.x, min.y, max.z);  // 뒤-왼-아래
        Vector3 v5 = new Vector3(max.x, min.y, max.z);  // 뒤-오-아래
        Vector3 v6 = new Vector3(max.x, max.y, max.z);  // 뒤-오-위
        Vector3 v7 = new Vector3(min.x, max.y, max.z);  // 뒤-왼-위

        // 1) 바닥면 4개 선
        Gizmos.DrawLine(v0, v1);
        Gizmos.DrawLine(v1, v2);
        Gizmos.DrawLine(v2, v3);
        Gizmos.DrawLine(v3, v0);

        // 2) 천장면 4개 선
        Gizmos.DrawLine(v4, v5);
        Gizmos.DrawLine(v5, v6);
        Gizmos.DrawLine(v6, v7);
        Gizmos.DrawLine(v7, v4);

        // 3) 바닥과 천장을 연결하는 4개 모서리 선
        Gizmos.DrawLine(v0, v4);
        Gizmos.DrawLine(v1, v5);
        Gizmos.DrawLine(v2, v6);
        Gizmos.DrawLine(v3, v7);
    }

    public override void updateGizmos()
    {
        updatePointGizmos();
    }
    protected void updatePointGizmos()
    {
        Debug.Log("updateGizmos is TODO");
        //pointGizmos = YCB_AreaFunction.sampling_grid_arr(this, pointGizmosInter);
    }
    protected void updateMeshGizmos()
    {
        Vector3 min = samplingAreaMin();
        Vector3 max = samplingAreaMax();

        // 박스의 8개 정점 계산
        Vector3[] vertices = new Vector3[8];
        vertices[0] = new Vector3(min.x, min.y, min.z);
        vertices[1] = new Vector3(max.x, min.y, min.z);
        vertices[2] = new Vector3(max.x, max.y, min.z);
        vertices[3] = new Vector3(min.x, max.y, min.z);
        vertices[4] = new Vector3(min.x, min.y, max.z);
        vertices[5] = new Vector3(max.x, min.y, max.z);
        vertices[6] = new Vector3(max.x, max.y, max.z);
        vertices[7] = new Vector3(min.x, max.y, max.z);

        // 삼각형 인덱스 배열 (각 면 2개의 삼각형, 총 12개의 삼각형)
        int[] triangles = new int[]
        {
        // 바닥면
        0, 1, 2,
        0, 2, 3,
        // 천장면
        4, 6, 5,
        4, 7, 6,
        // 앞면
        0, 4, 5,
        0, 5, 1,
        // 뒷면
        3, 2, 6,
        3, 6, 7,
        // 왼쪽면
        0, 3, 7,
        0, 7, 4,
        // 오른쪽면
        1, 5, 6,
        1, 6, 2
        };

        meshGizmos = new Mesh();
        meshGizmos.vertices = vertices;
        meshGizmos.triangles = triangles;
        meshGizmos.RecalculateNormals();
    }
    protected override void updateTransformChild ()
    {
        locateA = null;
        locateB = null;
        foreach(Transform e in transform)
        {
            if(locateA==null)
            {
                locateA = e;
            }
            else if (locateB ==null)
            {
                locateB = e;
            }
            else
            {
                break;
            }
        }
    }
}
