using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
//using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Progress_Axis : MonoBehaviour
{
    #region 축 관련
    private string updatetime;
    private string gripperload;
    private string axis1load;
    private string axis2load;
    private string axis3load;
    private string axis4load;
    private string axis5load;
    private string axis6load;

    public TMP_Text gripper_text;
    public TMP_Text axis1_text;
    public TMP_Text axis2_text;
    public TMP_Text axis3_text;
    public TMP_Text axis4_text;
    public TMP_Text axis5_text;
    public TMP_Text axis6_text;

    public Image gripper_circle;
    public Image axis1_circle;
    public Image axis2_circle;
    public Image axis3_circle;
    public Image axis4_circle;
    public Image axis5_circle;
    public Image axis6_circle;

    public Material Red_Material;

    public Material text_Red_Material;
    public Material text_white_Material;

    public Material Gripper_Material;
    public Material Axis_Material;
    #endregion

    #region 두께 관련
    public Text Thick;
    #endregion

    public float GameTime = 0;
    public float TotalTime;

    public void Update()
    {     

        try
        {
            GameTime += Time.deltaTime;
            if (GameTime > TotalTime)
            {
                Progress_Status();
                GameTime = 0;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    #region Config 관련
    //Config 파일 경로 세팅
    private string GetConfigFilePath()
    {
        return Path.Combine(Application.streamingAssetsPath, "config.ini");
    }

    //Config 파일 정보 리딩
    public void LoadConfig()
    {
        try
        {
            string configPath = Path.Combine(Application.dataPath, "config.ini");

            if (File.Exists(configPath))
            {
                // INI 파일 읽기
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);

                // INI 파일에서 값 읽어오기
                updatetime = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_UPDATETIME").InnerText;
                gripperload = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_GRIPPERLOAD").InnerText;
                axis1load = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS1LOAD").InnerText;
                axis2load = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS2LOAD").InnerText;
                axis3load = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS3LOAD").InnerText;
                axis4load = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS4LOAD").InnerText;
                axis5load = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS5LOAD").InnerText;
                axis6load = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS6LOAD").InnerText;

                //갱신시간 
                TotalTime = Convert.ToInt32(updatetime);
            }
            else
            {
                Debug.LogError("Config file not found!");
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    //씬 시작 시 제일 우선적으로 실행 (Config 세팅)
    private void Awake()
    {
        try
        {
            LoadConfig();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
    #endregion

    //DB 정보 리딩 (축, 두께) 및 부하율 초과시 색 변경
    public void Progress_Status()
    {
        mysql_wrapper mysql = new mysql_wrapper("fourone", "YOUNGDONG");
        DataTable dt = null;
        try
        {
            if (mysql.Connect())
            {
                string query = $@"
                                    SELECT *
                                    FROM dt_condition
                                    WHERE CONDITION_NAME = 'gripper_burden' 
                                       OR CONDITION_NAME = 'axis1_burden'
                                       OR CONDITION_NAME = 'axis2_burden'
                                       OR CONDITION_NAME = 'axis3_burden'
                                       OR CONDITION_NAME = 'axis4_burden'
                                       OR CONDITION_NAME = 'axis5_burden'
                                       OR CONDITION_NAME = 'axis6_burden'
                                       OR CONDITION_NAME = 'fabric_thick'
                                ";
                dt = mysql.Select(query);

                #region 축 관련
                float gripper_value = (float)dt.Rows[6]["CONDITION_VALUE"];
                float axis1_value = (float)dt.Rows[0]["CONDITION_VALUE"];
                float axis2_value = (float)dt.Rows[1]["CONDITION_VALUE"];
                float axis3_value = (float)dt.Rows[2]["CONDITION_VALUE"];
                float axis4_value = (float)dt.Rows[3]["CONDITION_VALUE"];
                float axis5_value = (float)dt.Rows[4]["CONDITION_VALUE"];
                float axis6_value = (float)dt.Rows[5]["CONDITION_VALUE"];

                gripper_text.text = gripper_value.ToString() + "%";
                axis1_text.text = axis1_value.ToString() + "%";
                axis2_text.text = axis2_value.ToString() + "%";
                axis3_text.text = axis3_value.ToString() + "%";
                axis4_text.text = axis4_value.ToString() + "%";
                axis5_text.text = axis5_value.ToString() + "%";
                axis6_text.text = axis6_value.ToString() + "%";

                gripper_circle.fillAmount = gripper_value / 100;
                axis1_circle.fillAmount = axis1_value / 100;
                axis2_circle.fillAmount = axis2_value / 100;
                axis3_circle.fillAmount = axis3_value / 100;
                axis4_circle.fillAmount = axis4_value / 100;
                axis5_circle.fillAmount = axis5_value / 100;
                axis6_circle.fillAmount = axis6_value / 100;

                if (gripper_value > (float.Parse(gripperload)))
                {
                    Axis_Burden("Gripper", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Gripper", Axis_Material, text_white_Material);
                }

                if (axis1_value > (float.Parse(axis1load)))
                {
                    Axis_Burden("Axis_1", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Axis_1", Axis_Material, text_white_Material);
                }

                if (axis2_value > (float.Parse(axis2load)))
                {
                    Axis_Burden("Axis_2", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Axis_2", Axis_Material, text_white_Material);
                }

                if (axis3_value > (float.Parse(axis3load)))
                {
                    Axis_Burden("Axis_3", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Axis_3", Axis_Material, text_white_Material);
                }

                if (axis4_value > (float.Parse(axis4load)))
                {
                    Axis_Burden("Axis_4", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Axis_4", Axis_Material, text_white_Material);
                }

                if (axis5_value > (float.Parse(axis5load)))
                {
                    Axis_Burden("Axis_5", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Axis_5", Axis_Material, text_white_Material);
                }

                if (axis6_value > (float.Parse(axis6load)))
                {
                    Axis_Burden("Axis_6", Red_Material, text_Red_Material);
                }
                else
                {
                    Axis_Burden("Axis_6", Axis_Material, text_white_Material);
                }
                #endregion

                //두께 세팅 (mm로 받아오기에 /10을 하여 cm로 변경
                float Thick_value = (float)dt.Rows[7]["CONDITION_VALUE"] / 10;
                Thick.text = Thick_value.ToString() + "cm";
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
        finally
        {
            mysql.Disconnect();
        }
    }

    //태그로 오브젝트 구분 및 붉은색으로 변경하는 함수
    public void Axis_Burden(string tag, Material objcolor, Material uicolor)
    {
        // 지정한 태그를 가진 모든 게임 오브젝트를 가져옵니다.
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

        // 각 게임 오브젝트의 머터리얼을 변경합니다.
        foreach (GameObject obj in objectsWithTag)
        {
            Renderer objRenderer = obj.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                //Debug.Log(objRenderer.name);
                objRenderer.material = objcolor;
            }

            Image image = obj.GetComponent<Image>();
            if (image != null)
            {
                image.material = uicolor; // 색상 변경
            }

            // UI 요소의 Text 컴포넌트를 찾습니다.
            Text text = obj.GetComponent<Text>();
            if (text != null)
            {
                text.material = uicolor; // 색상 변경
            }

            TMP_Text tmp = obj.GetComponent<TMP_Text>();
            if (tmp != null)
            {
                tmp.material = uicolor;
            }
        }
    }

}