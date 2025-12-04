using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class AxisCreator : MonoBehaviour
{
    [Tooltip("원통이 붙어 있는 Rigidbody들")]
    public List<Rigidbody> m_axis_rigid = new();

    [Tooltip("원통이랑 싱크로할 GameObject들")]
    public List<GameObject> m_axis_syncronize = new();

    [Header("옵션")]
    public bool is_synchronize = false;
    public bool is_gismos = false;

    [Header("Gizmos param")]
    public float sphereScale = 0.2f;     // 중심 구 크기
    public float lineScale = 1.0f;     // 선 길이 배율
    public Color sphereColor = Color.yellow;
    public Color axisColor = Color.cyan;

    private void OnDrawGizmos()
    {
        if (m_axis_rigid == null || m_axis_rigid.Count == 0) return;
        if (is_gismos)
        {
            foreach (var rb in m_axis_rigid)
            {
                if (rb == null) continue;

                // 1) 중심
                Vector3 center = get_center(rb);
                if (center == Vector3.zero) continue;           // 계산 실패 시 skip

                // 2) 축 방향 (월드)
                Quaternion q = get_axis(rb);
                Vector3 axis = (q * Vector3.forward).normalized;
                if (axis.sqrMagnitude < 1e-6f) continue;        // 축 계산 실패

                // 3) 그리기
                Gizmos.color = sphereColor;
                Gizmos.DrawSphere(center, sphereScale);

                Gizmos.color = axisColor;
                Gizmos.DrawLine(center - axis * lineScale,
                                center + axis * lineScale);
            }
        }
        if(is_synchronize)
        {
            SyncTransform();
        }
    }
    public void SyncListSize()
    {

        if(m_axis_rigid == null)
        {
            m_axis_syncronize = null;
            return;
        }
        if (m_axis_syncronize == null) m_axis_syncronize = new();
        while (m_axis_syncronize.Count < m_axis_rigid.Count)
        {
            m_axis_syncronize.Add(null);
        }
        while(m_axis_syncronize.Count > m_axis_rigid.Count)
        {
            int index = m_axis_syncronize.Count - 1;
            GameObject obj = m_axis_syncronize[index];
            if (obj !=null)
            {
                GameObject.Destroy(obj);
            }
            m_axis_syncronize.RemoveAt(index);
        }
    }
    public void SyncTransform()
    {
        SyncListSize();
        for (int i = 0; i < m_axis_rigid.Count; ++i)
        {
            Rigidbody rb = m_axis_rigid[i];
            if (rb == null) continue;

            Vector3 center = get_center(rb);
            if (center == Vector3.zero) continue;

            Quaternion axisQ = get_axis(rb);
            Vector3 axisDir = (axisQ * Vector3.forward).normalized;
            if (axisDir.sqrMagnitude < 1e-6f) continue;

            GameObject g = m_axis_syncronize[i];
            if (g == null)
            {
                g = new GameObject($"AxisSync_{i}");
                g.transform.SetParent(transform, false);
                m_axis_syncronize[i] = g;
            }

            g.transform.SetPositionAndRotation(center, axisQ);
        }
    }
    public Vector3 get_center (Rigidbody rb)
    {
        Mesh mesh = null;
        var mf = rb.GetComponent<MeshFilter>();
        if (mf != null) mesh = mf.sharedMesh;
        if (mesh == null)
        {
            var mc = rb.GetComponent<MeshCollider>();
            if (mc != null) mesh = mc.sharedMesh;
        }
        if (mesh == null || mesh.vertexCount < 3)
            return Vector3.zero;

        // 1) 로컬-버텍스 평균
        Vector3 sum = Vector3.zero;
        var verts = mesh.vertices;
        for (int i = 0; i < verts.Length; ++i)
            sum += verts[i];
        Vector3 centerLocal = sum / verts.Length;

        // 2) 로컬 → 월드
        return rb.transform.TransformPoint(centerLocal);
    }
    public Quaternion get_axis (Rigidbody rb)
    {
        Mesh mesh = null;
        var mf = rb.GetComponent<MeshFilter>();
        if (mf != null) mesh = mf.sharedMesh;
        if (mesh == null)
        {
            var mc = rb.GetComponent<MeshCollider>();
            if (mc != null) mesh = mc.sharedMesh;
        }
        if (mesh == null || mesh.vertexCount < 3)        
            return Quaternion.identity;

        var verts = mesh.vertices;
        int n = verts.Length;
        Vector3 c = Vector3.zero;
        foreach (var v in verts) c += v;
        c /= n;

        double xx = 0, xy = 0, xz = 0, yy = 0, yz = 0, zz = 0;
        foreach (var v0 in verts)
        {
            Vector3 d = v0 - c;
            xx += d.x * d.x; xy += d.x * d.y; xz += d.x * d.z;
            yy += d.y * d.y; yz += d.y * d.z;
            zz += d.z * d.z;
        }
        // 대칭 행렬
        double[,] M = {
        { xx, xy, xz },
        { xy, yy, yz },
        { xz, yz, zz }
    };

        Vector3 v1 = PowerIter(M, 32);                          
        double λ1 = Rayleigh(M, v1);
        Deflate(M, v1, λ1);                                      
        Vector3 v2 = PowerIter(M, 32);                           
        v1.Normalize(); v2.Normalize();
        Vector3 axisLocal = Vector3.Cross(v1, v2).normalized;   

        Vector3 axisWorld = rb.transform.TransformDirection(axisLocal);
        if (axisWorld.sqrMagnitude < 1e-6f) return Quaternion.identity;

        return Quaternion.FromToRotation(Vector3.forward, axisWorld.normalized);
    }

    static Vector3 PowerIter(double[,] A, int iters)
    {
        Vector3 v = new Vector3(1, 0, 0);            
        for (int k = 0; k < iters; ++k)
        {
            // w = A·v
            Vector3 w = new Vector3(
                (float)(A[0, 0] * v.x + A[0, 1] * v.y + A[0, 2] * v.z),
                (float)(A[1, 0] * v.x + A[1, 1] * v.y + A[1, 2] * v.z),
                (float)(A[2, 0] * v.x + A[2, 1] * v.y + A[2, 2] * v.z)
            );
            v = w.normalized;
        }
        return v;
    }

    static double Rayleigh(double[,] A, Vector3 v)
    {
        return A[0, 0] * v.x * v.x + 2 * A[0, 1] * v.x * v.y + 2 * A[0, 2] * v.x * v.z +
                A[1, 1] * v.y * v.y + 2 * A[1, 2] * v.y * v.z +
                A[2, 2] * v.z * v.z;
    }

    static void Deflate(double[,] A, Vector3 v, double λ)
    {
        double kx = λ * v.x, ky = λ * v.y, kz = λ * v.z;
        A[0, 0] -= kx * v.x; A[0, 1] -= kx * v.y; A[0, 2] -= kx * v.z;
        A[1, 0] -= ky * v.x; A[1, 1] -= ky * v.y; A[1, 2] -= ky * v.z;
        A[2, 0] -= kz * v.x; A[2, 1] -= kz * v.y; A[2, 2] -= kz * v.z;
    }
}
