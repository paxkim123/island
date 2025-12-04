using UnityEngine;

public class IKTargetUpdate : IKController
{
    public Transform m_target;  // 목표 위치·자세를 갖고 있는 Transform

    [Header("요청 주기")]
    public float requestHz = 10f;
    float nextTime;


    void Update()
    {
        if (Time.time >= nextTime)
        {
            RequestIK();
            nextTime = Time.time + 1f / requestHz;
        }
    }

    // baseLink 기준 로컬 위치 (x,y,z) 반환
    public override Vector3 get_position
    {
        get
        {
            return root_joints.transform
                           .InverseTransformPoint(m_target.position);
        }
    }

    // baseLink 기준 로컬 RPY(라디안) 반환
    public override Vector3 get_orientation
    {
        get
        {
            Quaternion rel = Quaternion.Inverse(root_joints.transform.rotation)
                             * m_target.rotation;
            Vector3 e = rel.eulerAngles;
            // Unity eulerAngles 는 0~360이므로 -180~180으로 보정
            e.x = Mathf.DeltaAngle(0, e.x);
            e.y = Mathf.DeltaAngle(0, e.y);
            e.z = Mathf.DeltaAngle(0, e.z);
            return e * Mathf.Deg2Rad;
        }
    }
}
