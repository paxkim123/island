using UnityEngine;
using UnityEngine.Perception.Randomization.Randomizers;
using System.Collections.Generic;
using JetBrains.Annotations;

public class CustomObjectRandomizer : Randomizer
{
    [Header("랜덤화 대상 오브젝트")]
    public List<Transform> objectsToRandomize = new List<Transform>();

    [Header("랜덤화 대상 오브젝트의 부모")]
    public List<Transform> parentsObjectsToRandomize = new List<Transform>();

    [Header("카메라 설정")]
    public Camera sceneCamera;

    public Transform targetCamera;

    [Header("타겟 배치 영역")]
    public Transform minArea;
    public Transform maxArea;

    [Header("카메라 파라미터")]
    public Vector2 distanceRange = new Vector2(5, 15);
    public Vector2 fovRange = new Vector2(40f, 80f);

    protected override void OnIterationStart()
    {

        // 1. 대상(Target) 위치를 랜덤으로 결정 (minArea ~ maxArea)
        if (minArea == null || maxArea == null)
        {
            Debug.LogWarning("minArea 또는 maxArea가 할당되어 있지 않습니다.");
            return;
        }
        float targetPosX = Random.Range(minArea.position.x, maxArea.position.x);
        float targetPosY = Random.Range(minArea.position.y, maxArea.position.y);
        float targetPosZ = Random.Range(minArea.position.z, maxArea.position.z);
        Vector3 randomTargetPos = new Vector3(targetPosX, targetPosY, targetPosZ);
        targetCamera.position = randomTargetPos;
        Debug.Log($"타겟 위치 설정됨: {randomTargetPos}");

        // 2. 카메라 랜덤화: targetCamera를 중심으로, 랜덤 거리와 랜덤 각도로 카메라를 배치
        if (sceneCamera == null)
        {
            Debug.LogWarning("SceneCamera가 할당되어 있지 않습니다.");
            return;
        }
        float radius = Random.Range(distanceRange.x, distanceRange.y);
        float azimuth = Random.Range(0f, 360f);
        float elevation = Random.Range(-90f, 90f);
        float azimuthRad = azimuth * Mathf.Deg2Rad;
        float elevationRad = elevation * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(
            radius * Mathf.Cos(elevationRad) * Mathf.Cos(azimuthRad),
            radius * Mathf.Sin(elevationRad),
            radius * Mathf.Cos(elevationRad) * Mathf.Sin(azimuthRad)
        );
        sceneCamera.transform.position = targetCamera.position + offset;
        sceneCamera.transform.LookAt(targetCamera);
        sceneCamera.fieldOfView = Random.Range(fovRange.x, fovRange.y);
        Debug.Log($"카메라 배치됨: 위치={sceneCamera.transform.position}, 거리={radius}, azimuth={azimuth}, elevation={elevation}");

        // 3. objectsToRandomize의 오브젝트들을 minArea ~ maxArea 영역 내에서 랜덤 배치
        foreach (Transform obj in objectsToRandomize)
        {
            if (obj == null)
                continue;
            float objPosX = Random.Range(minArea.position.x, maxArea.position.x);
            float objPosY = Random.Range(minArea.position.y, maxArea.position.y);
            float objPosZ = Random.Range(minArea.position.z, maxArea.position.z);
            obj.position = new Vector3(objPosX, objPosY, objPosZ);
            obj.rotation = Random.rotation;
        }

        // 4. parentsObjectsToRandomize의 오브젝트들도 동일한 영역 내에서 랜덤 배치
        foreach (Transform parent in parentsObjectsToRandomize)
        {
            foreach (Transform obj in parent)
            {
                if (obj == null)
                continue;
                float parentPosX = Random.Range(minArea.position.x, maxArea.position.x);
                float parentPosY = Random.Range(minArea.position.y, maxArea.position.y);
                float parentPosZ = Random.Range(minArea.position.z, maxArea.position.z);
                obj.position = new Vector3(parentPosX, parentPosY, parentPosZ);
                obj.rotation = Random.rotation;
            }
        }
    }
}
