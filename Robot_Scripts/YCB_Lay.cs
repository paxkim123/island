using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Unity.Jobs;
using UnityEngine.Rendering;
using Unity.Collections;

public class YCB_Lay : MonoBehaviour
{
    private Vector3 baseDirectionRay = Vector3.down;
    private Vector3 baseDirectionX = Vector3.left;
    private Vector3 baseDirectionZ = Vector3.back;

    public int m_resolution_X = 100;
    public int m_resolution_height = 5;
    public int m_resolution_Z = 100;

    public Transform m_locateA;
    public Transform m_locateB;

    public bool m_flow = true;
    public bool m_is_layer_update = false;
    public bool m_is_layer_gizmos = false;
    public bool m_is_union_gizmos = false;
    public bool m_is_locate_gizmos = false;
    public bool m_is_arrow_gizmos = false;

    public float m_connect_gradient = 2f;

    private List<Layer> layer_list = new List<Layer>();
    private List<ParticleUnion> union_list = new List<ParticleUnion>();
    public Vector3 getDirectionRay { get { return (transform.rotation * baseDirectionRay).normalized; } }
    public Vector3 getDirectionX { get { return (transform.rotation * baseDirectionX).normalized; } }
    public Vector3 getDirectionZ { get { return (transform.rotation * baseDirectionZ).normalized; } }

    public Vector3 getLocateMax { get {
            float dotA = Vector3.Dot(m_locateA.position, getDirectionRay);
            float dotB = Vector3.Dot(m_locateB.position, getDirectionRay);
            if (dotA > dotB) return m_locateA.position;
            else return m_locateB.position;
        } }
    public Vector3 getLocateMin { get {
            float dotA = Vector3.Dot(m_locateA.position, getDirectionRay);
            float dotB = Vector3.Dot(m_locateB.position, getDirectionRay);
            if (dotA < dotB) return m_locateA.position;
            else return m_locateB.position;
        } }
    public float getHeight { get {

            float dotA = Vector3.Dot(m_locateA.position, getDirectionRay);
            float dotB = Vector3.Dot(m_locateB.position, getDirectionRay);
            return Mathf.Abs(dotA - dotB);
        } }
    public struct Layer
    {

        public Vector3 p0;
        public Vector3 p1;
        public Vector3 p2;
        public Vector3 p3;

        public Layer(Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3)
        {
            p0 = _p0;
            p1 = _p1;
            p2 = _p2;
            p3 = _p3;
        }
        public Layer(Vector3 _p0, Vector3 _p1, Vector3 _p2, Vector3 _p3, Vector3 height)
        {
            p0 = _p0 + height;
            p1 = _p1 + height;
            p2 = _p2 + height;
            p3 = _p3 + height;
        }
    }
    public struct ParticleUnion
    {
        int x_offset;
        int z_offset;
        Vector3?[,] vector_grid;
        public bool isIndex(int x, int z)
        {
            int localX = x - x_offset;
            int localZ = z - z_offset;
            return
                localX >= 0 && localZ >= 0 &&
                localX < vector_grid.GetLength(0) &&
                localZ < vector_grid.GetLength(1) &&
                vector_grid[localX, localZ].HasValue;
        }
        public Vector3? getParticle(int x, int z)
        {
            int localX = x - x_offset;
            int localZ = z - z_offset;
            if (localX < 0 || localZ < 0 ||
                localX >= vector_grid.GetLength(0) ||
                localZ >= vector_grid.GetLength(1))
                return null;
            return vector_grid[localX, localZ];
        }
        private void Compression()
        {
            int w = vector_grid.GetLength(0);
            int h = vector_grid.GetLength(1);

            int minX = w, minZ = h, maxX = -1, maxZ = -1;

            for (int i = 0; i < w; i++)
                for (int j = 0; j < h; j++)
                    if (vector_grid[i, j].HasValue)
                    {
                        if (i < minX) minX = i;
                        if (j < minZ) minZ = j;
                        if (i > maxX) maxX = i;
                        if (j > maxZ) maxZ = j;
                    }
            if (maxX < 0)
                return;

            int newW = maxX - minX + 1;
            int newH = maxZ - minZ + 1;
            var newGrid = new Vector3?[newW, newH];

            for (int i = 0; i < newW; i++)
                for (int j = 0; j < newH; j++)
                    newGrid[i, j] = vector_grid[minX + i, minZ + j];

            // 오프셋 보정: 원래 전역 좌표에서 local 인덱스를 계산하던 기준점을 이동
            x_offset += minX;
            z_offset += minZ;
            vector_grid = newGrid;
        }
        public ParticleUnion(Vector3?[,] _vector_grid, int _x_offset, int _z_offset)
        {
            x_offset = _x_offset;
            z_offset = _z_offset;
            vector_grid = _vector_grid;
            Compression();
        }
    }
    public Layer getLayer(int index, bool flow = true)
    {
        Vector3 max = getLocateMax;
        Vector3 min = getLocateMin;
        Vector3 lay = getDirectionRay;

        if (flow)
        {
            // max 를 min 으로 끌어 내려서 height 만큼 높임
            Vector3 pmax = max - Vector3.Dot(max - min, lay) * lay;
            Vector3 diff = pmax - min;
            Vector3 XLocate = min + Vector3.Dot(diff, getDirectionX) * getDirectionX;
            Vector3 ZLocate = min + Vector3.Dot(diff, getDirectionZ) * getDirectionZ;
            Vector3 height = getDirectionRay * ((float)index / (float)m_resolution_height) * getHeight;
            return new Layer(min, XLocate, pmax, ZLocate, height);
        }
        else
        {
            // min 을 max 만큼 끌어 올려서 height 만큼 낮춤
            Vector3 pmin = min - Vector3.Dot(min - max, lay) * lay;
            Vector3 diff = pmin - max;
            Vector3 XLocate = max + Vector3.Dot(diff, getDirectionX) * getDirectionX;
            Vector3 ZLocate = max + Vector3.Dot(diff, getDirectionZ) * getDirectionZ;
            Vector3 height = -getDirectionRay * ((float)index / (float)m_resolution_height) * getHeight;
            return new Layer(max, ZLocate, pmin, XLocate, height);
        }
    }

    public List<Layer> getLayerList(bool flow = true)
    {
        layer_list.Clear();
        for (int i = 0; i < m_resolution_height; i++)
        {
            layer_list.Add(getLayer(i, flow));
        }
        return layer_list;
    }
    public Vector3?[,] createVectorGrid2Layer(Layer input_layer)
    {
        int sizeX = m_resolution_X+1;
        int sizeZ = m_resolution_Z+1;


        Vector3 vectorX = input_layer.p1 - input_layer.p0;
        Vector3 vectorZ = input_layer.p3 - input_layer.p0;
        Vector3 rayDir = getDirectionRay;

        Vector3?[,] vector_grid = new Vector3?[sizeX, sizeZ];

        for (int x = 0; x < sizeX; x++) {
            for (int z = 0; z < sizeZ; z++)
            {

                float perX = (float)x / (float)m_resolution_X;
                float perZ = (float)z / (float)m_resolution_Z;
                vector_grid[x, z] = null;
                Vector3 rayStart = input_layer.p0 + vectorX * perX + vectorZ * perZ;
                if (Physics.Raycast(rayStart, rayDir, out RaycastHit hitInfo, Mathf.Infinity))
                {
                    // 충돌 지점 저장
                    vector_grid[x, z] = hitInfo.point;
                }
            }
        }
        return vector_grid;
    }

    private void OnValidate()
    {
        getLayerList(m_flow);
    }
    private void OnDrawGizmos()
    {
        if(m_is_layer_update)
        {
            getLayerList(m_flow);
        }
        if(m_is_layer_gizmos)
        {
            if(layer_list != null)
            {
                for (int i = 0; i< layer_list.Count; i++)
                {
                    Layer layer = layer_list[i];

                    // Mesh 생성: 사각형의 네 꼭짓점을 두 개의 삼각형으로 분할합니다.
                    Mesh mesh = new Mesh();
                    Vector3[] vertices = new Vector3[4]
                    {
                    layer.p0,
                    layer.p1,
                    layer.p2,
                    layer.p3
                    };

                    // 삼각형 인덱스 (정점의 순서가 시계방향 또는 반시계방향이면 노멀 계산에 영향을 미칠 수 있음)
                    int[] triangles = new int[12]
                    {
                    0, 2, 1,  // 첫 번째 삼각형
                    0, 3, 2,   // 두 번째 삼각형
                    0, 1, 2,
                    0, 2, 3
                    };

                    mesh.vertices = vertices;
                    mesh.triangles = triangles;
                    mesh.RecalculateNormals();

                    // 예시로 각 레이어마다 색상을 점진적으로 변화시킵니다.
                    Gizmos.color = new Color(1, 0, 0, 0.3f);

                    // 불투명한 메쉬 형태로 그리기
                    Gizmos.DrawMesh(mesh);
                }
            }
        }
        if(m_is_union_gizmos)
        {
        }
    }
}
