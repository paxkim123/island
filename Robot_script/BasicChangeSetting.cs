using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class BasicChangeSetting : MonoBehaviour
{
    public InputField Update_Time;
    public InputField Gripper_Load;
    public InputField Axis1_Load;
    public InputField Axis2_Load;
    public InputField Axis3_Load;
    public InputField Axis4_Load;
    public InputField Axis5_Load;
    public InputField Axis6_Load;


    private string updatetime;
    private string gripperload;
    private string axis1load;
    private string axis2load;
    private string axis3load;
    private string axis4load;
    private string axis5load;
    private string axis6load;



    private string GetConfigFilePath()
    {
        return Path.Combine(Application.streamingAssetsPath, "config.ini");
    }

    private void LoadConfig()
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

    private void IniReadValue()
    {
        try
        {
            Update_Time.text = updatetime;
            Gripper_Load.text = gripperload;
            Axis1_Load.text = axis1load;
            Axis2_Load.text = axis2load;
            Axis3_Load.text = axis3load;
            Axis4_Load.text = axis4load;
            Axis5_Load.text = axis5load;
            Axis6_Load.text = axis6load;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    public void IniWriteValue()
    {
        try
        {
            updatetime = Update_Time.text;
            gripperload = Gripper_Load.text;
            axis1load = Axis1_Load.text;
            axis2load = Axis2_Load.text;
            axis3load = Axis3_Load.text;
            axis4load = Axis4_Load.text;
            axis5load = Axis5_Load.text;
            axis6load = Axis6_Load.text;

            string configPath = Path.Combine(Application.dataPath, "config.ini");

            if (File.Exists(configPath))
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);

                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_UPDATETIME").InnerText = updatetime;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_GRIPPERLOAD").InnerText = gripperload;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS1LOAD").InnerText = axis1load;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS2LOAD").InnerText = axis2load;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS3LOAD").InnerText = axis3load;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS4LOAD").InnerText = axis4load;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS5LOAD").InnerText = axis5load;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_AXIS6LOAD").InnerText = axis6load;

                xmlDoc.Save(configPath);

                Debug.Log("INI file updated successfully.");

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



    private void Start()
    {
        try
        {
            IniReadValue();
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}