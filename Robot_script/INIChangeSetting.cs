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

public class INIChangeSetting : MonoBehaviour
{
    public InputField IP_Field;
    public InputField PORT_Field;
    public InputField NAME_Field;
    public InputField ID_Field;
    public InputField PW_Field;
    public InputField ROOT_Field;


    private string ipAddress;
    private string dbId;
    private string dbPw;
    private string dbName;
    private string dbPort;
    private string dbRoot;


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
                ipAddress = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_IP").InnerText;
                dbPort = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_PORT").InnerText;
                dbName = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_DB").InnerText;
                dbId = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_ID").InnerText;
                dbPw = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_PW").InnerText;
                dbRoot = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_ROOT").InnerText;

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
            PW_Field.contentType = InputField.ContentType.Password;
            IP_Field.text = ipAddress;
            PORT_Field.text = dbPort;
            NAME_Field.text = dbName;
            ID_Field.text = dbId;
            PW_Field.text = dbPw;
            ROOT_Field.text = dbRoot;

  
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
            ipAddress = IP_Field.text;
            dbPort = PORT_Field.text;
            dbName = NAME_Field.text;
            dbId = ID_Field.text;
            dbPw = PW_Field.text;
            dbRoot = ROOT_Field.text;

            string configPath = Path.Combine(Application.dataPath, "config.ini");

            if (File.Exists(configPath))
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(configPath);

                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_IP").InnerText = ipAddress;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_PORT").InnerText = dbPort;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_DB").InnerText = dbName;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_ID").InnerText = dbId;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_PW").InnerText = dbPw;
                xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_ROOT").InnerText = dbRoot;

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