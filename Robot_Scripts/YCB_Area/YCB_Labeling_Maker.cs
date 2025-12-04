using UnityEngine;
using UnityEngine.Perception.GroundTruth.LabelManagement;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.Perception.GroundTruth;

public class YCB_Labeling_Maker : MonoBehaviour
{
    public GameObject rootObject;

    public void labelingAllObjs()
    {
        if (rootObject == null)
        {
            Debug.LogWarning("Root Object가 할당되어 있지 않습니다!");
            return;
        }

        // rootObject 하위의 모든 자식 Transform (비활성 오브젝트 포함)을 가져옴.
        Transform[] allTransforms = rootObject.GetComponentsInChildren<Transform>(true);

        foreach (Transform t in allTransforms)
        {
            // rootObject 자신은 제외.
            if (t.gameObject == rootObject)
                continue;

            // 만약 자식의 직속 부모가 있다면, 부모에 Labeling 컴포넌트가 붙어 있는지 확인 후 제거.
            if (t.parent != null)
            {
                Labeling parentLabeling = t.parent.GetComponent<Labeling>();
                if (parentLabeling != null)
                {
#if UNITY_EDITOR
                    DestroyImmediate(parentLabeling);
#else
                    Destroy(parentLabeling);
#endif
                    Debug.Log("부모(" + t.parent.gameObject.name + ")의 기존 라벨링 제거됨.");
                }
            }

            // 자식 오브젝트에 기존 Labeling 컴포넌트가 있으면 제거.
            Labeling existingLabeling = t.GetComponent<Labeling>();
            if (existingLabeling != null)
            {
#if UNITY_EDITOR
                DestroyImmediate(existingLabeling);
#else
                Destroy(existingLabeling);
#endif
            }

            // 자식 오브젝트에 새 Labeling 컴포넌트 추가.
            Labeling newLabeling = t.gameObject.AddComponent<Labeling>();
            if (newLabeling.labels == null)
            {
                newLabeling.labels = new List<string>();
            }
            else
            {
                newLabeling.labels.Clear();
            }

            // 자식의 직속 부모 이름을 라벨로 사용.
            if (t.parent != null)
            {
                string parentName = t.parent.gameObject.name;
                newLabeling.labels.Add(parentName);
                Debug.Log("라벨 추가됨: " + parentName + " (자식: " + t.gameObject.name + ")");
            }
        }
    }
}
