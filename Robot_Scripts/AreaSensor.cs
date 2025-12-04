using UnityEngine;

[ExecuteInEditMode]
public class AreaSensor : MonoBehaviour
{
    public Transform m_right_up;
    public Transform m_right_down;

    public Transform m_left_up;
    public Transform m_left_down;

    public int m_laye_count = 10;

    public bool m_is_diagonal = false;
    public bool m_is_beeline = false;

    public Color m_color = Color.red;

    // 씬 상에서 센서 영역을 시각화하기 위한 Gizmos 그리기
    private void OnDrawGizmos()
    {
        // 네 개의 코너가 모두 할당되지 않았다면 그리지 않음
        if (m_left_up == null || m_left_down == null || m_right_up == null || m_right_down == null)
            return;

        Gizmos.color = m_color;

        // beeline: 단순히 가로 방향으로만 선을 그림
        if (m_is_beeline)
        {
            for (int i = 0; i <= m_laye_count; i++)
            {
                float t = (float)i / m_laye_count;
                Vector3 start = Vector3.Lerp(m_left_down.position, m_left_up.position, t);
                Vector3 end = Vector3.Lerp(m_right_down.position, m_right_up.position, t);
                Gizmos.DrawLine(start, end);
            }
        }

        // diagonal: 각 레이어마다 한 칸 위/아래 레이어와 교차하는 대각선 그리기
        if (m_is_diagonal)
        {
            for (int i = 0; i < m_laye_count; i++)
            {
                float t_current = (float)i / m_laye_count;
                float t_next = (float)(i + 1) / m_laye_count;

                Vector3 left_current = Vector3.Lerp(m_left_down.position, m_left_up.position, t_current);
                Vector3 right_current = Vector3.Lerp(m_right_down.position, m_right_up.position, t_current);
                Vector3 left_next = Vector3.Lerp(m_left_down.position, m_left_up.position, t_next);
                Vector3 right_next = Vector3.Lerp(m_right_down.position, m_right_up.position, t_next);

                // 좌측 현재 레이어 -> 우측 다음 레이어
                Gizmos.DrawLine(left_current, right_next);
                // 우측 현재 레이어 -> 좌측 다음 레이어
                Gizmos.DrawLine(right_current, left_next);
            }
        }
    }
}
