using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class YCB_Prefab_Maker : MonoBehaviour
{
    // 루트 폴더 경로 (예: "C:/CD/UNITY/PerceptionHDRP/Assets/Resources")
    public string rootFolderPath;
    // 탐색할 폴더 이름 (예: "google_512k")
    public string searchFolderName;
    // 각 폴더 안에서 찾을 파일명 (예: "textured")
    public string targetFileName;

    // 생성된 오브젝트를 자식으로 붙일 부모 오브젝트
    public GameObject rootObject;

    public void ImportAllObjs()
    {
#if UNITY_EDITOR
        // rootFolderPath 내에서 searchFolderName과 일치하는 모든 폴더를 재귀적으로 찾음.
        string[] matchingFolders = Directory.GetDirectories(rootFolderPath, searchFolderName, SearchOption.AllDirectories);

        // Application.dataPath와 targetFilePath를 비교할 때 슬래시를 통일
        string normalizedDataPath = Application.dataPath.Replace("\\", "/");

        foreach (string folder in matchingFolders)
        {
            // 각 폴더 안에서 targetFileName 파일의 전체 경로 생성
            string targetFilePath = Path.Combine(folder, targetFileName);
            // 경로를 슬래시로 통일
            string normalizedTargetPath = targetFilePath.Replace("\\", "/");

            if (File.Exists(normalizedTargetPath))
            {
                // OS 파일 경로를 Unity 에셋 경로로 변환 (normalizedDataPath를 "Assets"로 대체)
                string assetPath = normalizedTargetPath.Replace(normalizedDataPath, "Assets");

                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (prefab == null)
                {
                    // LoadAssetAtPath로 찾지 못하면 메인 에셋을 로드 시도
                    UnityEngine.Object mainAsset = AssetDatabase.LoadMainAssetAtPath(assetPath);
                    if (mainAsset is GameObject)
                    {
                        prefab = mainAsset as GameObject;
                    }
                }

                if (prefab != null)
                {
                    // 상위 폴더의 이름 추출 (예: "Assets/Resources/512k/077_rubiks_cube/google_512k"에서 "077_rubiks_cube")
                    string parentFolderPath = Path.GetDirectoryName(folder);
                    string parentFolderName = Path.GetFileName(parentFolderPath);

                    // rootObject의 자식으로 prefab 인스턴스 생성
                    GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, rootObject.transform);
                    instance.name = parentFolderName;
                    Debug.Log("Instantiated prefab with name: " + parentFolderName);
                }
                else
                {
                    Debug.LogWarning("Prefab not found at: " + assetPath);
                }
            }
            else
            {
                Debug.LogWarning("File not found: " + normalizedTargetPath);
            }
        }
#else
        Debug.LogWarning("ImportAllObjs는 에디터에서만 사용할 수 있습니다.");
#endif
    }
}