using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class YCB_AreaSeg : YCB_Area , IYCB_Area
{
    // --- Public 변수 ---
    public AXIS axis;
    public List<Transform> segmentation = new List<Transform>();

    // --- 캐시 변수 ---
    // polygon은 해당 축에 맞게 2D 평면(예: AXIS.X인 경우 YZ 평면)에서의 꼭짓점 배열
    private Vector2[] polygon = null;
    // ceiling: segmentation 기준 최대값, floor: 최소값
    private float ceiling = float.MinValue;
    private float floor = float.MaxValue;

    // --- isArea 함수 ---
    // 캐시된 polygon, floor, ceiling을 이용해 position이 영역 내에 있는지 검사
    public bool isArea(Vector3 position)
    {
        // 캐시 값이 유효하지 않으면(즉, 갱신이 안된 경우) false
        if (ceiling == float.MinValue || ceiling == float.MaxValue) return false;
        if (floor == float.MaxValue || floor == float.MinValue) return false;
        if (polygon == null || polygon.Length < 3) return false;

        switch (axis)
        {
            case AXIS.X:
                {
                    // X 좌표가 [floor, ceiling] 내에 있는지
                    if (position.x < floor || position.x > ceiling) { return false; }
                    // YZ 평면상의 다각형 내부 검사 (Test point: (position.y, position.z))
                    Vector2 testPoint = new Vector2(position.y, position.z);
                    return IsPointInPolygon(testPoint, polygon);
                }
            case AXIS.Y:
                {
                    if (position.y < floor || position.y > ceiling) { return false; }
                    // ZX 평면상의 검사 (Test point: (position.z, position.x))
                    Vector2 testPoint = new Vector2(position.z, position.x);
                    return IsPointInPolygon(testPoint, polygon);
                }
            case AXIS.Z:
                {
                    if (position.z < floor || position.z > ceiling) { return false; }
                    // XY 평면상의 검사 (Test point: (position.x, position.y))
                    Vector2 testPoint = new Vector2(position.x, position.y);
                    return IsPointInPolygon(testPoint, polygon);
                }
        }
        return false;
    }

    // --- 샘플링 관련 API (원래 구현 방식 유지) ---
    public List<Vector3> samplingArea()
    {
        return YCB_AreaFunction.sampling_grid(this);
    }

    public Vector3 samplingAreaMax()
    {
        float maxX = float.MinValue;
        float maxY = float.MinValue;
        float maxZ = float.MinValue;

        foreach (Transform t in segmentation)
        {
            if (maxX < t.position.x) maxX = t.position.x;
            if (maxY < t.position.y) maxY = t.position.y;
            if (maxZ < t.position.z) maxZ = t.position.z;
        }
        return new Vector3(maxX, maxY, maxZ);
    }

    public Vector3 samplingAreaMin()
    {
        float minX = float.MaxValue;
        float minY = float.MaxValue;
        float minZ = float.MaxValue;

        foreach (Transform t in segmentation)
        {
            if (minX > t.position.x) minX = t.position.x;
            if (minY > t.position.y) minY = t.position.y;
            if (minZ > t.position.z) minZ = t.position.z;
        }
        return new Vector3(minX, minY, minZ);
    }

    // --- 다각형 내부 검사 함수 ---
    // 해당 점이 polygon(꼭짓점 배열) 내부에 있는지 ray-casting 기법을 통해 검사
    private bool IsPointInPolygon(Vector2 point, Vector2[] polygonArray)
    {
        int n = polygonArray.Length;
        bool inside = false;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            Vector2 pi = polygonArray[i];
            Vector2 pj = polygonArray[j];

            if (((pi.y > point.y) != (pj.y > point.y)) &&
                (point.x < (pj.x - pi.x) * (point.y - pi.y) / (pj.y - pi.y) + pi.x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    // --- 캐시 갱신 함수들 ---
    // segmentation 기반으로 ceiling(최대값)을 계산
    private void get_ceiling()
    {
        ceiling = float.MinValue;
        switch (axis)
        {
            case AXIS.X:
                {
                    float result = float.MinValue;
                    foreach (Transform e in segmentation)
                    {
                        if (result < e.position.x) result = e.position.x;
                    }
                    ceiling = result;
                    break;
                }
            case AXIS.Y:
                {
                    float result = float.MinValue;
                    foreach (Transform e in segmentation)
                    {
                        if (result < e.position.y) result = e.position.y;
                    }
                    ceiling = result;
                    break;
                }
            case AXIS.Z:
                {
                    float result = float.MinValue;
                    foreach (Transform e in segmentation)
                    {
                        if (result < e.position.z) result = e.position.z;
                    }
                    ceiling = result;
                    break;
                }
        }
    }

    // segmentation 기반으로 floor(최소값)을 계산
    private void get_floor()
    {
        floor = float.MaxValue;
        switch (axis)
        {
            case AXIS.X:
                {
                    float result = float.MaxValue;
                    foreach (Transform e in segmentation)
                    {
                        if (result > e.position.x) result = e.position.x;
                    }
                    floor = result;
                    break;
                }
            case AXIS.Y:
                {
                    float result = float.MaxValue;
                    foreach (Transform e in segmentation)
                    {
                        if (result > e.position.y) result = e.position.y;
                    }
                    floor = result;
                    break;
                }
            case AXIS.Z:
                {
                    float result = float.MaxValue;
                    foreach (Transform e in segmentation)
                    {
                        if (result > e.position.z) result = e.position.z;
                    }
                    floor = result;
                    break;
                }
        }
    }

    // segmentation 기반으로 2D 다각형 꼭짓점 배열을 생성(축에 따라 평면이 달라짐)
    private void get_polygon()
    {
        List<Vector2> polygonTemp = new List<Vector2>();
        switch (axis)
        {
            case AXIS.X:
                {
                    // YZ 평면상의 다각형: (t.position.y, t.position.z)
                    foreach (Transform t in segmentation)
                    {
                        polygonTemp.Add(new Vector2(t.position.y, t.position.z));
                    }
                    break;
                }
            case AXIS.Y:
                {
                    // ZX 평면상의 다각형: (t.position.z, t.position.x)
                    foreach (Transform t in segmentation)
                    {
                        polygonTemp.Add(new Vector2(t.position.z, t.position.x));
                    }
                    break;
                }
            case AXIS.Z:
                {
                    // XY 평면상의 다각형: (t.position.x, t.position.y)
                    foreach (Transform t in segmentation)
                    {
                        polygonTemp.Add(new Vector2(t.position.x, t.position.y));
                    }
                    break;
                }
        }
        polygon = polygonTemp.ToArray();
    }

    // --- Gizmos 관련 처리 ---
    // 업데이트 시 Gizmos 표시용 포인트를 갱신 (원래 구현 유지)
    public override void updateGizmos()
    {
        updatePointGizmos();
    }

    public void updatePointGizmos()
    {
        Debug.Log("updateGizmos is TODO");
        //pointGizmos = YCB_AreaFunction.sampling_grid_arr(this, pointGizmosInter);
    }

    public void updateMeshGizmos()
    {
        // segmentation의 개수가 3개 미만이면 다각형 메시를 구성할 수 없음
        if (segmentation == null || segmentation.Count < 3)
        {
            meshGizmos = null;
            return;
        }

        meshGizmos = new Mesh();
        int count = segmentation.Count;
        Vector3[] vertices = new Vector3[count];

        // 선택된 축에 따라 2D 평면에 맞게 vertex 구성
        for (int i = 0; i < count; i++)
        {
            switch (axis)
            {
                case AXIS.X:
                    // X축은 고정 → YZ 평면상의 vertex (첫 번째 Transform의 X값 사용)
                    vertices[i] = new Vector3(segmentation[0].position.x, segmentation[i].position.y, segmentation[i].position.z);
                    break;
                case AXIS.Y:
                    vertices[i] = new Vector3(segmentation[i].position.x, segmentation[0].position.y, segmentation[i].position.z);
                    break;
                case AXIS.Z:
                    vertices[i] = new Vector3(segmentation[i].position.x, segmentation[i].position.y, segmentation[0].position.z);
                    break;
            }
        }

        // 단순 삼각형 팬(triangle fan) 방식으로 다각형 메시 구성
        int triangleCount = (count - 2) * 3;
        int[] triangles = new int[triangleCount];
        for (int i = 0; i < count - 2; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        meshGizmos.vertices = vertices;
        meshGizmos.triangles = triangles;
        meshGizmos.RecalculateNormals();
    }

    // --- transform 갱신 ---
    // 자식 Transform들이 변경될 때 segmentation 리스트를 갱신하고 캐시도 업데이트
    protected override void updateTransformChild()
    {
        segmentation.Clear();
        foreach (Transform e in transform)
        {
            segmentation.Add(e);
        }
        // segmentation이 갱신되었으므로 캐시도 새로 계산
        get_ceiling();
        get_floor();
        get_polygon();
    }

    // --- OnDrawGizmos ---
    // 디버깅용 Gizmos로 샘플링된 포인트들을 작은 큐브로 시각화함
    void OnDrawGizmos()
    {
        // 기본 검사: Gizmos 사용 여부 및 segmentation 최소 조건 체크
        if (!isGizmos || segmentation == null || segmentation.Count < 2)
            return;

        // 원하는 Gizmos 색상 설정 (예: colorGizmos 변수에 지정된 색상)
        Gizmos.color = colorGizmos;

        // floorPoints, ceilingPoints 배열 생성
        Vector3[] floorPoints = new Vector3[segmentation.Count];
        Vector3[] ceilingPoints = new Vector3[segmentation.Count];

        // floor와 ceiling 값은 segmentation의 최소/최대 값으로 설정합니다.
        // (이 값은 보통 updateTransformChild나 별도의 캐시 업데이트에서 미리 계산할 수 있음)
        Vector3 areaMin = samplingAreaMin();
        Vector3 areaMax = samplingAreaMax();

        switch (axis)
        {
            case AXIS.X:
                {
                    // X축: YZ 평면으로 투영
                    // 바닥: X값은 areaMin.x, 천장: X값은 areaMax.x
                    float fixedFloor = areaMin.x;
                    float fixedCeiling = areaMax.x;
                    for (int i = 0; i < segmentation.Count; i++)
                    {
                        // segmentation[i].position의 Y, Z 값은 그대로 사용
                        Vector3 segPos = segmentation[i].position;
                        floorPoints[i] = new Vector3(fixedFloor, segPos.y, segPos.z);
                        ceilingPoints[i] = new Vector3(fixedCeiling, segPos.y, segPos.z);
                    }
                }
                break;
            case AXIS.Y:
                {
                    // Y축: ZX 평면으로 투영
                    float fixedFloor = areaMin.y;
                    float fixedCeiling = areaMax.y;
                    for (int i = 0; i < segmentation.Count; i++)
                    {
                        Vector3 segPos = segmentation[i].position;
                        floorPoints[i] = new Vector3(segPos.x, fixedFloor, segPos.z);
                        ceilingPoints[i] = new Vector3(segPos.x, fixedCeiling, segPos.z);
                    }
                }
                break;
            case AXIS.Z:
                {
                    // Z축: XY 평면으로 투영
                    float fixedFloor = areaMin.z;
                    float fixedCeiling = areaMax.z;
                    for (int i = 0; i < segmentation.Count; i++)
                    {
                        Vector3 segPos = segmentation[i].position;
                        floorPoints[i] = new Vector3(segPos.x, segPos.y, fixedFloor);
                        ceilingPoints[i] = new Vector3(segPos.x, segPos.y, fixedCeiling);
                    }
                }
                break;
        }

        // 1. 바닥 다각형 그리기 (floorPoints)
        for (int i = 0; i < floorPoints.Length; i++)
        {
            int nextIndex = (i + 1) % floorPoints.Length;
            Gizmos.DrawLine(floorPoints[i], floorPoints[nextIndex]);
        }

        // 2. 천장 다각형 그리기 (ceilingPoints)
        for (int i = 0; i < ceilingPoints.Length; i++)
        {
            int nextIndex = (i + 1) % ceilingPoints.Length;
            Gizmos.DrawLine(ceilingPoints[i], ceilingPoints[nextIndex]);
        }

        // 3. 바닥과 천장을 연결하는 선(수직 라인) 그리기
        for (int i = 0; i < floorPoints.Length; i++)
        {
            Gizmos.DrawLine(floorPoints[i], ceilingPoints[i]);
        }
    }

}
public enum AXIS
{
    X , Y , Z
}
