using UnityEngine;
using TMPro;
using UnityEditor;
public class PoseEstimation : MonoBehaviour
{
    public TextMeshProUGUI Text;

    public float width  = 1.0f;
    public float length = 1.0f;
    public float height = 1.0f;
    public float thickness = 1.0f;

    public enum POSE_ESTIMATION_MODE{ WLH_MODE, VERTEX_MODE };

    public POSE_ESTIMATION_MODE pose_estimatino_mode;

    public Transform vertexTopFrontRight;
    public Transform vertexTopFrontLeft;
    public Transform vertexTopBackRight;
    public Transform vertexTopBackLeft;

    public Transform vertexBottomFrontRight;
    public Transform vertexBottomFrontLeft;
    public Transform vertexBottomBackRight;
    public Transform vertexBottomBackLeft;

    public Transform edgeTopFront;
    public Transform edgeTopRight;
    public Transform edgeTopBack;
    public Transform edgeTopLeft;

    public Transform edgeBottomFront;
    public Transform edgeBottomRight;
    public Transform edgeBottomBack;
    public Transform edgeBottomLeft;

    public Transform edgeFrontLeft;
    public Transform edgeFrontRight;
    public Transform edgeBackRight;
    public Transform edgeBackLeft;

    private void OnDrawGizmos()
    {
        if (pose_estimatino_mode == POSE_ESTIMATION_MODE.WLH_MODE)
        {
            if (edgeTopFront != null &&
            edgeTopRight != null &&
            edgeTopBack != null &&
            edgeTopLeft != null &&
            edgeBottomFront != null &&
            edgeBottomRight != null &&
            edgeBottomBack != null &&
            edgeBottomLeft != null &&
            edgeFrontLeft != null &&
            edgeFrontRight != null &&
            edgeBackRight != null &&
            edgeBackLeft != null)
            {
                UpdateEdgesWLH();
            }
        }
        else if (pose_estimatino_mode == POSE_ESTIMATION_MODE.VERTEX_MODE)
        {
            if (edgeTopFront != null &&
            edgeTopRight != null &&
            edgeTopBack != null &&
            edgeTopLeft != null &&
            edgeBottomFront != null &&
            edgeBottomRight != null &&
            edgeBottomBack != null &&
            edgeBottomLeft != null &&
            edgeFrontLeft != null &&
            edgeFrontRight != null &&
            edgeBackRight != null &&
            edgeBackLeft != null)
            {
                UpdateEdgesVertex();
            }
        }
        UpdateText();
    }
    private void UpdateText()
    {
        Text.text = $"ROLL\r\n{transform.position.ToString()}";
    }
    void UpdateEdgesVertex()
    {
        // Helper 로직: edge Transform, 두 vertex Transform 을 받아 위치·회전·스케일 설정
        void PositionEdge(Transform edge, Transform v1, Transform v2)
        {
            Vector3 p1 = v1.position;
            Vector3 p2 = v2.position;
            Vector3 mid = (p1 + p2) * 0.5f;
            Vector3 dir = p2 - p1;

            edge.position = mid;
            if (dir.sqrMagnitude > Mathf.Epsilon)
            {
                // 유니티 큐브는 Y축(0,1,0) 방향이 길이 방향이므로
                edge.up = dir.normalized;
            }
            // Y축 스케일을 두 점 간 거리로, X/Z는 thickness 로
            edge.localScale = new Vector3(thickness, dir.magnitude, thickness);
        }

        // Top face
        PositionEdge(edgeTopFront, vertexTopFrontLeft, vertexTopFrontRight);
        PositionEdge(edgeTopBack, vertexTopBackRight, vertexTopBackLeft);
        PositionEdge(edgeTopRight, vertexTopFrontRight, vertexTopBackRight);
        PositionEdge(edgeTopLeft, vertexTopFrontLeft, vertexTopBackLeft);

        //Bottom face
        PositionEdge(edgeBottomFront ,  vertexBottomFrontLeft ,  vertexBottomFrontRight);
        PositionEdge(edgeBottomBack  ,  vertexBottomBackRight ,  vertexBottomBackLeft);
        PositionEdge(edgeBottomRight ,  vertexBottomFrontRight , vertexBottomBackRight);
        PositionEdge(edgeBottomLeft  ,  vertexBottomFrontLeft  , vertexBottomBackLeft);


        PositionEdge(edgeFrontRight, vertexTopFrontRight, vertexBottomFrontRight);
        PositionEdge(edgeFrontLeft, vertexTopFrontLeft, vertexBottomFrontLeft);
        PositionEdge(edgeBackRight, vertexTopBackRight, vertexBottomBackRight);
        PositionEdge(edgeBackLeft, vertexTopBackLeft, vertexBottomBackLeft);
    }
    void UpdateEdgesWLH()
    {
        float hx = width * 0.5f;
        float hy = height * 0.5f;
        float hz = length * 0.5f;
        float t = thickness;

        // 기준 Origin
        Vector3 origin = transform.position;

        // ─── Top Front ───────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(0, hy, hz);
            edgeTopFront.position = origin + transform.rotation * offset;
            edgeTopFront.rotation = transform.rotation * Quaternion.identity;
            edgeTopFront.localScale = new Vector3(width, t, t);
        }

        // ─── Top Back ────────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(0, hy, -hz);
            edgeTopBack.position = origin + transform.rotation * offset;
            edgeTopBack.rotation = transform.rotation * Quaternion.identity;
            edgeTopBack.localScale = new Vector3(width, t, t);
        }

        // ─── Top Right ───────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(hx, hy, 0);
            edgeTopRight.position = origin + transform.rotation * offset;
            edgeTopRight.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            edgeTopRight.localScale = new Vector3(length, t, t);
        }

        // ─── Top Left ────────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(-hx, hy, 0);
            edgeTopLeft.position = origin + transform.rotation * offset;
            edgeTopLeft.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            edgeTopLeft.localScale = new Vector3(length, t, t);
        }

        // ─── Bottom Front ────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(0, -hy, hz);
            edgeBottomFront.position = origin + transform.rotation * offset;
            edgeBottomFront.rotation = transform.rotation * Quaternion.identity;
            edgeBottomFront.localScale = new Vector3(width, t, t);
        }

        // ─── Bottom Back ─────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(0, -hy, -hz);
            edgeBottomBack.position = origin + transform.rotation * offset;
            edgeBottomBack.rotation = transform.rotation * Quaternion.identity;
            edgeBottomBack.localScale = new Vector3(width, t, t);
        }

        // ─── Bottom Right ────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(hx, -hy, 0);
            edgeBottomRight.position = origin + transform.rotation * offset;
            edgeBottomRight.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            edgeBottomRight.localScale = new Vector3(length, t, t);
        }

        // ─── Bottom Left ─────────────────────────────────────────────────────────
        {
            Vector3 offset = new Vector3(-hx, -hy, 0);
            edgeBottomLeft.position = origin + transform.rotation*offset;
            edgeBottomLeft.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            edgeBottomLeft.localScale = new Vector3(length, t, t);
        }
        Vector3[] verticalLocalOffsets = {
    new Vector3(-hx,  0,  hz),  // front left
    new Vector3( hx,  0,  hz),  // front right
    new Vector3( hx,  0, -hz),  // back right
    new Vector3(-hx,  0, -hz),  // back left
};


        Transform[] verticalEdges = {
        edgeFrontLeft,
        edgeFrontRight,
        edgeBackRight,
        edgeBackLeft
    };
        for (int i = 0; i < 4; i++)
        {
            // 로컬 오프셋을 월드 좌표로 변환
            Vector3 worldPos = transform.TransformPoint(verticalLocalOffsets[i]);

            verticalEdges[i].position = worldPos;
            // 박스 전체 회전을 그대로 적용
            verticalEdges[i].rotation = transform.rotation;
            // Y축으로 늘어나도록 scale (thickness, height, thickness)
            verticalEdges[i].localScale = new Vector3(thickness, height, thickness);
        }
    }

}
