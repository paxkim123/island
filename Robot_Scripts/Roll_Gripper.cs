using UnityEngine;

public class Roll_Gripper : MonoBehaviour
{
    protected readonly float m_gripper_max_length = 20f;
    protected readonly float m_gripper_gripped_length = 14.2f;
    protected readonly float m_sleeve_close_angle = 90;
    protected readonly float m_sleeve_open_angle = 0;


    public float m_gripper_target_length = 20f;
    public float m_gripper_speed  = 10f;
    public float m_sleeve_target_angle   = 20f;
    public float m_sleeve_speed   = 10f;

    private float m_gripper_current_length;
    private float m_sleeve_current_angle;



    public Transform m_gripper_right;
    public Transform m_gripper_left;

    public Transform m_sleeve_front;
    public Transform m_sleeve_back;

    [Header("Crank")]
    public Transform m_crank_sleeve_front_right;
    public Transform m_crank_sleeve_front_left;
    public Transform m_crank_sleeve_back_right;
    public Transform m_crank_sleeve_back_left;


    public Transform m_crank_frame_front_right;
    public Transform m_crank_frame_front_left;
    public Transform m_crank_frame_back_right;
    public Transform m_crank_frame_back_left;

    private float m_start_angle_z_crank_front_right;
    private float m_start_angle_z_crank_front_left;
    private float m_start_angle_z_crank_back_right;
    private float m_start_angle_z_crank_back_left;

    void Start()
    {
        h_crank_start();
        m_gripper_current_length = m_gripper_max_length;
        m_sleeve_current_angle = m_sleeve_open_angle;
    }
    // Update is called once per frame
    void Update()
    {

        h_gripper_update();
        h_sleeve_update();
        h_crank_update();
    }
    private void h_crank_start()
    {
        m_start_angle_z_crank_front_right = GetLocalZtoTarget(m_crank_sleeve_front_right, m_crank_frame_front_right );
        m_start_angle_z_crank_front_left  = GetLocalZtoTarget(m_crank_sleeve_front_left , m_crank_frame_front_left  );
        m_start_angle_z_crank_back_right  = GetLocalZtoTarget(m_crank_sleeve_back_right , m_crank_frame_back_right  );
        m_start_angle_z_crank_back_left   = GetLocalZtoTarget(m_crank_sleeve_back_left  , m_crank_frame_back_left  );
    }
    private void h_crank_update()
    {
        SyncCrank(m_crank_sleeve_front_right, m_crank_frame_front_right, m_start_angle_z_crank_front_right,0);
        SyncCrank(m_crank_sleeve_front_left, m_crank_frame_front_left, m_start_angle_z_crank_front_left,0);
        SyncCrank(m_crank_sleeve_back_right, m_crank_frame_back_right, m_start_angle_z_crank_back_right,0);
        SyncCrank(m_crank_sleeve_back_left, m_crank_frame_back_left, m_start_angle_z_crank_back_left,0);


        SyncCrank(m_crank_frame_front_right, m_crank_sleeve_front_right,  m_start_angle_z_crank_front_right,180);
        SyncCrank(m_crank_frame_front_left, m_crank_sleeve_front_left,  m_start_angle_z_crank_front_left,180);


        SyncCrank(m_crank_frame_back_right, m_crank_sleeve_back_right,  m_start_angle_z_crank_back_right,190);
        SyncCrank(m_crank_frame_back_left, m_crank_sleeve_back_left, m_start_angle_z_crank_back_left,190);
    }
    private void h_gripper_update()
    {
        
        m_gripper_target_length = Mathf.Clamp(
            m_gripper_target_length, 0f, m_gripper_max_length);

        m_gripper_current_length = Mathf.MoveTowards(
            m_gripper_current_length,
            m_gripper_target_length,
            m_gripper_speed * Time.deltaTime);

        float half = 0.5f * (m_gripper_max_length - m_gripper_current_length);

        Vector3 posR = new Vector3(0, 0, -half); 
        m_gripper_right.localPosition = posR;

        Vector3 posL = new Vector3(0, 0, half);
        m_gripper_left.localPosition = posL;
    }

    private void h_sleeve_update()
    {
        m_sleeve_target_angle = Mathf.Clamp(
        m_sleeve_target_angle, m_sleeve_close_angle, m_sleeve_open_angle);

        // 보간
        m_sleeve_current_angle = Mathf.MoveTowards(
            m_sleeve_current_angle,
            m_sleeve_target_angle,
            m_sleeve_speed * Time.deltaTime);

        // Z만 교체 ─ X·Y 유지
        void SetZ(Transform t)
        {
            Vector3 eul = t.localEulerAngles;  // 현재 각도 가져오기
            eul.z = -m_sleeve_current_angle;    // Z만 덮어쓰기
            t.localEulerAngles = eul;          // 다시 넣기
        }

        SetZ(m_sleeve_front);
        SetZ(m_sleeve_back);
    }
    void SyncCrank(Transform sleeve, Transform frame, float startAngle, float offset)
    {
        float cur = GetLocalZtoTarget(sleeve, frame);

        float delta = Mathf.DeltaAngle(startAngle, cur);

        Vector3 eul = FindMesh(sleeve).localEulerAngles;
        eul.z = delta + offset;
        FindMesh(sleeve).localEulerAngles = eul;
    }
    float GetLocalZtoTarget(Transform sleeve, Transform frame)
    {
        Vector3 local = sleeve.InverseTransformPoint(frame.position);
        local.z = 0;
        if (local.sqrMagnitude < 1e-6f) return 0f;
        return Mathf.Atan2(local.y, local.x) * Mathf.Rad2Deg;
    }

    Transform FindMesh(Transform sleevePivot)
    {
        // 이름이 "Mesh"인 자식이 있으면 사용, 없으면 첫 번째 자식
        var mesh = sleevePivot.Find("Mesh");
        if (mesh == null && sleevePivot.childCount > 0)
            mesh = sleevePivot.GetChild(0);
        return mesh;
    }

    public void h_action_griper_close()
    {

    }
    public void h_action_griper_grip()
    {

    }
    public void h_action_griper_open()
    {

    }

    public void h_action_sleeve_open()
    {

    }
    public void h_action_sleeve_close()
    { 

    }
}
