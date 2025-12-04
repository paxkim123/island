using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class IKController : MonoBehaviour
{
    // IK 서버 주소
    public readonly string ikServerUrl = "http://localhost:8000/ikpy";
    public ArticulationBody[] joints;

    public ArticulationBody root_joints;

    public virtual Vector3 get_position
    {
        get { return new Vector3(0, 0, 0); }
    }
    public virtual Vector3 get_orientation
    {
        get { return new Vector3(0, 0, 0); }
    }
    // 서버 응답 구조
    [System.Serializable]
    public class IKResponse
    {
        public List<float> joint_angles;
    }

    // IK 요청 데이터 구조
    [System.Serializable]
    public class IKRequest
    {
        public float[] position;
        public float[] orientation;
    }

    // IK 요청 실행
    public void RequestIK()
    {
        StartCoroutine(RequestIKCoroutine());
    }
    IEnumerator RequestIKCoroutine()
    {
        // 1. 위치 추출
        Vector3 pos = get_position;
        Vector3 rpy = get_orientation;

        // 3. 요청 데이터 생성
        IKRequest reqData = new IKRequest
        {
            position = new float[] { pos.x, pos.y, pos.z },
            orientation = new float[] { rpy.x, rpy.y, rpy.z }
        };
        string json = JsonUtility.ToJson(reqData);

        using (UnityWebRequest www = new UnityWebRequest(ikServerUrl, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("IK 서버 통신 실패: " + www.error);
            }
            else
            {
                IKResponse response = JsonUtility.FromJson<IKResponse>(www.downloadHandler.text);
                Debug.Log($"{response.joint_angles[0]} - {response.joint_angles[1]} - {response.joint_angles[2]} - " +
                    $"{response.joint_angles[3]} - {response.joint_angles[4]} - {response.joint_angles[5]}" );
                ApplyJointAngles(response.joint_angles);
            }
        }
    }

    [Header("Drive Settings")]
    [Tooltip("관절을 움직이기 위한 스티프니스")]
    public float driveStiffness = 10000f;
    [Tooltip("관절을 제어할 때의 댐핑")]
    public float driveDamping = 100f;
    [Tooltip("관절이 견딜 수 있는 최대 토크")]
    public float driveForceLimit = 1000f;
    void Awake()
    {
        root_joints.immovable = true;
    }
    // 관절에 각도 적용
    void ApplyJointAngles(List<float> angles)
    {
        for (int i = 0; i < joints.Length && i < angles.Count; i++)
        {
            var joint = joints[i];
            float targetDeg = angles[i] * Mathf.Rad2Deg;

            // 어느 축이 LockedMotion인지 찍어보자
            Debug.Log(
                $"{joint.name} locks → " +
                $"twist:{joint.twistLock}, swingY:{joint.swingYLock}, swingZ:{joint.swingZLock}"
            );

            // x축 회전 가능하면 xDrive
            if (joint.twistLock != ArticulationDofLock.LockedMotion)
            {
                var drive = joint.xDrive;
                drive.stiffness = driveStiffness;
                drive.damping = driveDamping;
                drive.forceLimit = driveForceLimit;
                drive.target = targetDeg;
                joint.xDrive = drive;
            }
            // 아니면 y축 (LockedMotion이 아니면 가능)
            else if (joint.swingYLock != ArticulationDofLock.LockedMotion)
            {
                var drive = joint.yDrive;
                drive.stiffness = driveStiffness;
                drive.damping = driveDamping;
                drive.forceLimit = driveForceLimit;
                drive.target = targetDeg;
                joint.yDrive = drive;
            }
            // 아니면 z축
            else if (joint.swingZLock != ArticulationDofLock.LockedMotion)
            {
                var drive = joint.zDrive;
                drive.stiffness = driveStiffness;
                drive.damping = driveDamping;
                drive.forceLimit = driveForceLimit;
                drive.target = targetDeg;
                joint.zDrive = drive;
            }
            else
            {
                Debug.LogWarning($"[{joint.name}] 회전 가능한 축 없음. URDF 설정 확인.");
            }
        }
    }
}
