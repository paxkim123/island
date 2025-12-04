using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Packing_Robot_Move : MonoBehaviour
{
    public GameObject[] RobotObjects;
    public GameObject[] MovingObjects;
    public Vector3[] TargetPositions;
    public Vector3[] Fabric_TargetPositions;

    #region 로봇 대차 도착 좌표
    private Vector3[] Robot_TargetPositions_1;
    private Quaternion[] Robot_TargetRotations_1;
    #endregion

    #region 파레트 원단 개수
    public GameObject[] Pallet_1;
    public GameObject[] Pallet_2;
    public GameObject[] Pallet_3;
    public GameObject[] Pallet_4;
    public GameObject[] Pallet_5;
    #endregion

    public GameObject[] Fabric_Object;

    public float positionSpeed = 0.1f;
    public float rotationSpeed = 0.1f;

    private float MoveStatus = -1f;
    private float RobotStatus = -1f;

    private float Pallet_Trigger = -1f;

    public float GameTime = 0;
    private string updatetime;
    public float TotalTime;


    void Start()
    {
        Array_Setting();
        LoadConfig();

        Pallet_Count(0, Pallet_1);
        Pallet_Count(1, Pallet_2);
        Pallet_Count(2, Pallet_3);
        Pallet_Count(3, Pallet_4);
        Pallet_Count(4, Pallet_5);
    }

    void Update()
    {
        PackingRobotMove();

        try
        {
            GameTime += Time.deltaTime;
            if (GameTime > TotalTime)
            {

                Pallet_Count(0, Pallet_1);
                Pallet_Count(1, Pallet_2);
                Pallet_Count(2, Pallet_3);
                Pallet_Count(3, Pallet_4);
                Pallet_Count(4, Pallet_5);
                GameTime = 0;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }

    }

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

    public void Array_Setting()
    {
        ////////////포지션/////////////////
        Robot_TargetPositions_1 = new Vector3[2];

        Robot_TargetPositions_1[0] = new Vector3(0, 0, -5f);

        ////////////회전 각도/////////////////
        Robot_TargetRotations_1 = new Quaternion[6];

        Robot_TargetRotations_1[0] = Quaternion.Euler(0, 0, 0);
        Robot_TargetRotations_1[1] = Quaternion.Euler(-70, 0, 0);
        Robot_TargetRotations_1[2] = Quaternion.Euler(70, 0, 0);
        Robot_TargetRotations_1[3] = Quaternion.Euler(0, 0, 0);
        Robot_TargetRotations_1[4] = Quaternion.Euler(0, 0, 0);
        Robot_TargetRotations_1[5] = Quaternion.Euler(0, 180, 0);
    }

    #region 로봇이 포장기 -> 대차 움직임 함수
    public void PackingRobotMove()
    {
        try
        {
            GameTime += Time.deltaTime;
            if (GameTime > TotalTime)
            {
                PhotoSensor();
                GameTime = 0;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
        

        switch (MoveStatus) //포장기 움직임
        {
            case 0:
                Fabric_Object[0].SetActive(true);
                AutoMove(new GameObject[] { MovingObjects[0], MovingObjects[6] }, new Vector3[] { TargetPositions[0], Fabric_TargetPositions[1] });
                break;
            case 1:
                AutoMove(new GameObject[] { MovingObjects[2], MovingObjects[0] }, new Vector3[] { TargetPositions[2], Vector3.zero });
                break;
            case 2:
                AutoMove(new GameObject[] { MovingObjects[1], MovingObjects[6] }, new Vector3[] { TargetPositions[1], Fabric_TargetPositions[2] });
                break;
            case 3:
                AutoMove(new GameObject[] { MovingObjects[3], MovingObjects[4], MovingObjects[2], MovingObjects[1] },
                          new Vector3[] { TargetPositions[3], TargetPositions[4], Vector3.zero, Vector3.zero });
                break;
            case 4:
                AutoMove(new GameObject[] { MovingObjects[3], MovingObjects[4], MovingObjects[6] },
                          new Vector3[] { new Vector3(0, 0, -0.15f), new Vector3(0, 0, -0.4f), Fabric_TargetPositions[3] });
                break;
            case 5:
                AutoMove(new GameObject[] { MovingObjects[5], MovingObjects[6] },
                          new Vector3[] { TargetPositions[5], Fabric_TargetPositions[4] });
                break;
            case 6:
                AutoMove(new GameObject[] { MovingObjects[6] }, new Vector3[] { Fabric_TargetPositions[5] });
                break;
            case 7:
                AutoMove(new GameObject[] { MovingObjects[5] }, new Vector3[] { Vector3.zero });
                break;
            case 8:
                Fabric_Object[0].transform.position = new Vector3(11.35f, 1.22f, 0);
                Fabric_Object[0].SetActive(false);
                Fabric_Object[1].SetActive(true);

                if (Pallet_Trigger == 1)  //1~5 대차로 로봇 움직임
                {
                    RobotMove(new Vector3(-6.3f, 0, 0), Robot_TargetPositions_1, Robot_TargetRotations_1, 0, Pallet_1);
                }
                else if (Pallet_Trigger == 2)
                {
                    RobotMove(new Vector3(-4.25f, 0, 0), Robot_TargetPositions_1, Robot_TargetRotations_1, 1, Pallet_2);
                }
                else if (Pallet_Trigger == 3)
                {
                    RobotMove(new Vector3(-2.25f, 0, 0), Robot_TargetPositions_1, Robot_TargetRotations_1, 2, Pallet_3);
                }
                else if (Pallet_Trigger == 4)
                {
                    RobotMove(new Vector3(-0.25f, 0, 0), Robot_TargetPositions_1, Robot_TargetRotations_1, 3, Pallet_4);
                }
                else if (Pallet_Trigger == 5)
                {
                    RobotMove(new Vector3(1.75f, 0, 0), Robot_TargetPositions_1, Robot_TargetRotations_1, 4, Pallet_5);
                }
                break;
            case 9:
                Fabric_Object[1].SetActive(false);
                PackMove(); break;
            default:
                Debug.Log("대기 상태");
                break;
        }
    }
    #endregion

    #region 로봇이 대차 -> 포장기 움직임 함수
    public void RobotMove(Vector3 FramePostiion, Vector3[] targetLocalPositions, Quaternion[] targetLocalRotations, int PalletNum, GameObject[] FabricCount )
    {
        if (RobotStatus == 0)
        {
            Robot_AutoMove_P_R(RobotObjects[0], RobotObjects[1], FramePostiion, targetLocalRotations[0]);
        }
        else if (RobotStatus == 1)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[3], RobotObjects[2] }, new Quaternion[] { targetLocalRotations[1], targetLocalRotations[2] });
        }
        else if (RobotStatus == 2)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[4] }, new Quaternion[] { targetLocalRotations[3] });
        }
        else if (RobotStatus == 3)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[5] }, new Quaternion[] { targetLocalRotations[4] });
        }
        else if (RobotStatus == 4)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[6] }, new Quaternion[] { targetLocalRotations[5] });
        }
        else if (RobotStatus == 5)
        {
            Robot_Auto_Move_P(new GameObject[] { RobotObjects[7] }, new Vector3[] { targetLocalPositions[0] });
        }
        else if (RobotStatus == 6)
        {
            Pallet_Count(PalletNum, FabricCount);
            RobotStatus = 0;
            MoveStatus += 1;
        }
    }
    #endregion

  

    #region 포장기 움직임 세팅 함수
    public void AutoMove(GameObject[] Move_Ps, Vector3[] targetLocalPositions)
    {
        bool allArrived = true;

        for (int i = 0; i < Move_Ps.Length; i++)
        {
            Vector3 currentLocalPosition = Move_Ps[i].transform.localPosition;
            Move_Ps[i].transform.localPosition = Vector3.MoveTowards(currentLocalPosition, targetLocalPositions[i], positionSpeed * Time.deltaTime);

            if (Vector3.Distance(currentLocalPosition, targetLocalPositions[i]) >= 0.01f)
            {
                allArrived = false;
            }
        }

        if (allArrived)
        {
            MoveStatus += 1;
        }
    }
    #endregion

    #region 로봇 -> 대차 움직임 세팅 함수
    public void Robot_Auto_Move_P(GameObject[] Move_Ps, Vector3[] targetLocalPositions)
    {
        bool allArrived = true;

        for (int i = 0; i < Move_Ps.Length; i++)
        {
            Vector3 currentLocalPosition = Move_Ps[i].transform.localPosition;
            Move_Ps[i].transform.localPosition = Vector3.MoveTowards(currentLocalPosition, targetLocalPositions[i], positionSpeed * Time.deltaTime);

            if (Vector3.Distance(currentLocalPosition, targetLocalPositions[i]) >= 0.01f )
            {
                allArrived = false;
            }
        }

        if (allArrived)
        {
            RobotStatus += 1;
        }
    }

    public void Robot_Auto_Move_R(GameObject[] Move_R, Quaternion[] targetLocalRotation)
    {
        bool allArrived = true;

        for (int i = 0; i < Move_R.Length; i++)
        {
            Quaternion currentLocalRotation = Move_R[i].transform.localRotation;

            Move_R[i].transform.localRotation = Quaternion.RotateTowards(currentLocalRotation, targetLocalRotation[i], rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(currentLocalRotation, targetLocalRotation[i]) >= 0.01f)
            {
                allArrived = false;
            }
        }

        if (allArrived)
        {
            RobotStatus += 1;
        }
    }

    public void Robot_AutoMove_P_R(GameObject frame, GameObject one, Vector3 targetLocalPosition, Quaternion targetLocalRotation)
    {
        bool allArrived = true;

        // Position movement
        Vector3 currentLocalPosition = frame.transform.localPosition;
        frame.transform.localPosition = Vector3.MoveTowards(currentLocalPosition, targetLocalPosition, positionSpeed * Time.deltaTime);
        if (Vector3.Distance(currentLocalPosition, targetLocalPosition) >= 0.01f)
        {
            allArrived = false;
        }

        // Rotation movement
        Quaternion currentLocalRotation = one.transform.localRotation;
        one.transform.localRotation = Quaternion.RotateTowards(currentLocalRotation, targetLocalRotation, rotationSpeed * Time.deltaTime);
        if (Quaternion.Angle(currentLocalRotation, targetLocalRotation) >= 0.01f)
        {
            allArrived = false;
        }

        if (allArrived)
        {
            RobotStatus += 1;
        }
    }
    #endregion

    #region 로봇 -> 포장기 움직임 세팅 함수
    public void PackMove()
    {
        if (RobotStatus == 0)
        {
            Robot_Auto_Move_P(new GameObject[] { RobotObjects[7] }, new Vector3[] { new Vector3(0, 0, 0) });
        }
        else if (RobotStatus == 1)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[6] }, new Quaternion[] { Quaternion.Euler(0, 180, 0) });
        }
        else if (RobotStatus == 2)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[5] }, new Quaternion[] { Quaternion.Euler(0, 0, 0) });
        }
        else if (RobotStatus == 3)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[4] }, new Quaternion[] { Quaternion.Euler(0, 0, 0) });
        }
        else if (RobotStatus == 4)
        {
            Robot_Auto_Move_R(new GameObject[] { RobotObjects[3], RobotObjects[2] }, new Quaternion[] { Quaternion.Euler(-50, 0, 0), Quaternion.Euler(50, 0, 0) });
        }
        else if (RobotStatus == 5)
        {
            Robot_Auto_Move_P(new GameObject[] { RobotObjects[7] }, new Vector3[] { new Vector3(0, 0, 0) });
        }
        else if (RobotStatus == 6)
        {
            Robot_AutoMove_P_R(RobotObjects[0], RobotObjects[1], new Vector3(2, 0, 0), Quaternion.Euler(0, -90, 0));
        }
        else if (RobotStatus == 7)
        {
            RobotStatus = 0;
            MoveStatus = -1f;
        }
    }
    #endregion

    #region DB에서 대차 원단 값 갖고와서 활성화

    public void Pallet_Count(int RowCount, GameObject[] FabricGroup)
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
                                    WHERE CONDITION_NAME = 'pallet_1_count' 
                                       OR CONDITION_NAME = 'pallet_2_count'
                                       OR CONDITION_NAME = 'pallet_3_count'
                                       OR CONDITION_NAME = 'pallet_4_count'
                                       OR CONDITION_NAME = 'pallet_5_count'

                                ";
                dt = mysql.Select(query);

                int pallet_count = Convert.ToInt32(dt.Rows[RowCount]["CONDITION_VALUE"]);

                //개수가 0이면 모델링 초기화하는 코드 짜야함

                for (int i = 0; i < 36; i++)
                {
                    
                    if (i < pallet_count) 
                    {
                        
                        FabricGroup[i].SetActive(true); 
                    }
                    else
                    {
                        FabricGroup[i].SetActive(false);
                    }                    
                }                
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

    #endregion


    public void PhotoSensor()
    {
        mysql_wrapper mysql = new mysql_wrapper("fourone", "YOUNGDONG");
        DataTable dt = null;
        try
        {
            if (mysql.Connect())
            {
                string query = $@"
                                    SELECT 
                                        dt.CONDITION_NAME, 
                                        dt.CONDITION_VALUE, 
                                        fh.PALETTE_POSITION, 
                                        fh.INPUT_DATE 
                                    FROM 
                                        dt_condition dt 
                                    CROSS JOIN 
                                        fabric_history fh 
                                    WHERE 
                                        dt.CONDITION_NAME = 'photo_sensor'
                                    ORDER BY 
                                        fh.INPUT_DATE DESC
                                    LIMIT 1;
                                ";
                dt = mysql.Select(query);

                int PhotoSensor_Value = Convert.ToInt32(dt.Rows[0]["CONDITION_VALUE"]);
                int PhotoSensor_Position = Convert.ToInt32(dt.Rows[0]["PALETTE_POSITION"]);

                //Debug.Log(MoveStatus);

                if (MoveStatus == -1f && PhotoSensor_Value == 1)
                {
                    //Debug.Log("작동");
                    MoveStatus = 0;
                    RobotStatus = 0;
                    Pallet_Trigger = PhotoSensor_Position;
                }
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
}