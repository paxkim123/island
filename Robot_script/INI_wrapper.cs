using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
//using MySql.Data.MySqlClient;

public class INI_wrapper
{
    XmlDocument xmlDoc = null;

    string root = string.Empty;

    public INI_wrapper(string _root, string filepath = "config.ini")
    {
        string configPath = Path.Combine(Application.dataPath, filepath);

        if (File.Exists(configPath))
        {
            root = "/" + _root + "/";
            xmlDoc = new XmlDocument();
            xmlDoc.Load(configPath);
        }
    }

    public string read(string key)
    {
        if (xmlDoc == null)
        {
            return "";
        }
        return xmlDoc.SelectSingleNode(root + key).InnerText;
    }

    public void write(string key, string value)
    {
        if (xmlDoc == null)
        {
            return;
        }
        xmlDoc.SelectSingleNode(root + key).InnerText = value;
    }
}
