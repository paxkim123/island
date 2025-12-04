using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Unity.Mathematics;

public class ERROR_UI : MonoBehaviour
{
    public GameObject all;
    public Text Error_Txt;

    public float GameTime = 0;
    public float TotalTime = 1;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      

        try
        {
            GameTime += Time.deltaTime;
            if (GameTime > TotalTime)
            {
                Alarm_Text();
                GameTime = 0;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

   public void Alarm_Text()
    {
        mysql_wrapper mysql = new mysql_wrapper("fourone", "YOUNGDONG");
        DataTable dt = null;
        try
        {
            if (mysql.Connect())
            {
                string query = $@"
                              SELECT *
                              from error_table
                              where alarm = '1'
                              order by update_date desc
                                ";
                dt = mysql.Select(query);

                if (dt.Rows.Count > 0)
                {
                    all.SetActive(true);

                    Error_Txt.text = "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Error_Txt.text += " - " + dt.Rows[i]["MEMO"].ToString() + "\n"; 
                    } 
                }
                else
                {
                    all.SetActive(false); 
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
