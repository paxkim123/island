using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using MySql.Data.MySqlClient;
using System.Diagnostics;

public class History_exe : MonoBehaviour
{
    //public string filePath; // 실행할 파일의 경로
    private string historyexe;

    public void OpenProgram_out()
    {
        Process.Start(historyexe); // 외부 프로그램 실행
    }


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
                historyexe = xmlDoc.SelectSingleNode("/fourone/YOUNGDONG_ROOT").InnerText;
                //UnityEngine.Debug.Log(dbRoot);
            }
            else
            {
                UnityEngine.Debug.LogError("Config file not found!");
            }
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
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
            UnityEngine.Debug.Log(e.ToString());
        }
    }

    private void Start()
    {
        try
        {
            LoadConfig();
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.Log(e.ToString());
        }
    }

}