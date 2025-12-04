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

public class UI_CMD : MonoBehaviour
{
    private string updatetime;

    public Text[] Fabric_Count;

    public Text[] pallet_1;
    public Text[] pallet_2;
    public Text[] pallet_3;
    public Text[] pallet_4;
    public Text[] pallet_5;

    public Text Now_Fabric;

    public float GameTime = 0;
    public float TotalTime;

    // Start is called before the first frame update
    void Start()
    {
        ERP_UI();
        LoadConfig();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            GameTime += Time.deltaTime;
            if (GameTime > TotalTime)
            {
                ERP_UI();
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

    public void ERP_UI()
    {
        mysql_wrapper mysql = new mysql_wrapper("fourone", "YOUNGDONG");
        DataTable dt = null;
        DataTable dt2 = null;
        DataTable dt3 = null;
        Dictionary<string, DataTable> paletteTables = new Dictionary<string, DataTable>();

        try
        {
            if (mysql.Connect())
            {              
                string query2 = $@"
                             SELECT 
                                    CONDITION_NAME, CONDITION_VALUE 
                              FROM 
                                    dt_condition
                              where 
                                    CONDITION_NAME = 'pallet_1_count' or CONDITION_NAME = 'pallet_2_count' or CONDITION_NAME = 'pallet_3_count'or CONDITION_NAME = 'pallet_4_count' or CONDITION_NAME = 'pallet_5_count'
                            ";
                dt2 = mysql.Select(query2);

                string pa1count = dt2.Rows[0]["CONDITION_VALUE"].ToString();
                string pa2count = dt2.Rows[1]["CONDITION_VALUE"].ToString();
                string pa3count = dt2.Rows[2]["CONDITION_VALUE"].ToString();
                string pa4count = dt2.Rows[3]["CONDITION_VALUE"].ToString();
                string pa5count = dt2.Rows[4]["CONDITION_VALUE"].ToString();                            



                string query = $@"
                            (SELECT 
                                fh.PALETTE_POSITION, PALETTE_NO, BARCODE, ITEM_NM, QTY, QTY_M, fh.INPUT_DATE
                             FROM 
                                fabric_history fh
                             WHERE 
                                fh.PALETTE_POSITION = '1'
                             LIMIT {pa1count})

                            UNION ALL

                            (SELECT 
                                fh.PALETTE_POSITION, PALETTE_NO, BARCODE, ITEM_NM, QTY, QTY_M, fh.INPUT_DATE
                             FROM 
                                fabric_history fh
                             WHERE 
                                fh.PALETTE_POSITION = '2'
                             LIMIT {pa2count})

                            UNION ALL

                            (SELECT 
                                fh.PALETTE_POSITION, PALETTE_NO, BARCODE, ITEM_NM, QTY, QTY_M, fh.INPUT_DATE
                             FROM 
                                fabric_history fh
                             WHERE 
                                fh.PALETTE_POSITION =  '3'
                             LIMIT {pa3count})
 
                             UNION ALL

                            (SELECT 
                                fh.PALETTE_POSITION, PALETTE_NO, BARCODE, ITEM_NM, QTY, QTY_M, fh.INPUT_DATE
                             FROM 
                                fabric_history fh
                             WHERE 
                                fh.PALETTE_POSITION =  '4'
                             LIMIT {pa4count})
 
                             UNION ALL

                            (SELECT 
                                fh.PALETTE_POSITION, PALETTE_NO, BARCODE, ITEM_NM, QTY, QTY_M, fh.INPUT_DATE
                             FROM 
                                fabric_history fh
                             WHERE 
                                fh.PALETTE_POSITION =  '5'
                             LIMIT {pa5count})

                            ORDER BY 
                                PALETTE_POSITION, INPUT_DATE DESC;

                            ";
                dt = mysql.Select(query);

                string query3 = $@"
                             select *
                            from fabric_history
                            order by INPUT_DATE DESC
                            limit 1
                            ";
                dt3 = mysql.Select(query3);

                Text[][] pallets = { pallet_1, pallet_2, pallet_3, pallet_4, pallet_5 };
                string[] counts = { pa1count, pa2count, pa3count, pa4count, pa5count };

                for (int i = 0; i < counts.Length; i++)
                {
                    if (counts[i] == "0")
                    {
                        for (int j = 0; j < pallets[i].Length; j++)
                        {
                            pallets[i][j].text = " ";
                        }
                    }
                }


                #region 하단 ui 파레트 정보
                // PALETTE_POSITION별로 DataTable을 나누기
                foreach (DataRow row in dt.Rows)
                {
                    string palettePosition = row["PALETTE_POSITION"].ToString();

                    // 해당 PALETTE_POSITION을 위한 DataTable이 없으면 새로 생성
                    if (!paletteTables.ContainsKey(palettePosition))
                    {
                        DataTable newTable = dt.Clone(); // 구조 복사 (컬럼 포함)
                        paletteTables[palettePosition] = newTable;
                    }

                    // 해당 PALETTE_POSITION의 테이블에 데이터 추가
                    paletteTables[palettePosition].ImportRow(row);
                }


                for(int a = 0; a < Fabric_Count.Length; a++)  
                {
                    Fabric_Count[a].text = "Palette " + (a+1) + "  -  " + dt2.Rows[a]["CONDITION_VALUE"].ToString() + "개";
                }


                // 각 PALETTE_POSITION에 해당하는 DataTable을 사용하여 추가 작업 수행 및 UI 업데이트
                foreach (var pair in paletteTables)
                {
                    string palettePosition = pair.Key;
                    DataTable table = pair.Value;
                    Text[] palletUI = null;

                    switch (palettePosition)
                    {
                        case "1":
                            palletUI = pallet_1;
                            break;
                        case "2":
                            palletUI = pallet_2;
                            break;
                        case "3":
                            palletUI = pallet_3;
                            break;
                        case "4":
                            palletUI = pallet_4;
                            break;
                        case "5":
                            palletUI = pallet_5;
                            break;
                        default:
                            Debug.LogWarning($"Unknown PALETTE_POSITION: {palettePosition}");
                            continue;
                    }

                    // dt2에서 해당 팔레트에 맞는 CONDITION_VALUE 찾기
                    string conditionName = $"pallet_{palettePosition}_count";
                    DataRow[] conditionRows = dt2.Select($"CONDITION_NAME = '{conditionName}'");

                 


                    int i = 0;
                    foreach (DataRow tableRow in table.Rows)
                    {
                        if (i >= palletUI.Length) break;

                        if (i < palletUI.Length)
                        {
                            string barcode = tableRow["BARCODE"].ToString().Substring(0, Math.Min(20, tableRow["BARCODE"].ToString().Length));
                            palletUI[i].text = barcode;
                            i++;
                        }

                        if (i < palletUI.Length)
                        {
                            string itemName = tableRow["ITEM_NM"].ToString().Substring(0, Math.Min(20, tableRow["ITEM_NM"].ToString().Length));
                            palletUI[i].text = itemName;
                            i++;
                        }

                        if (i < palletUI.Length)
                        {
                            string qty = tableRow["QTY"].ToString();
                            if (qty == "0.0")
                            {
                                qty = tableRow["QTY_M"].ToString();
                            }
                            qty = qty.Substring(0, Math.Min(10, qty.Length));
                            palletUI[i].text = qty;
                            i++;
                        }
                    }


                }
                #endregion

                string now_barcode = dt3.Rows[0]["BARCODE"].ToString();
                now_barcode = now_barcode.Length >= 15 ? now_barcode.Substring(0, 15) : now_barcode;

                string now_itemName = dt3.Rows[0]["ITEM_NM"].ToString();
                now_itemName = now_itemName.Length >= 15 ? now_itemName.Substring(0, 15) : now_itemName;

                string now_paletteNo = dt3.Rows[0]["PALETTE_NO"].ToString();
                now_paletteNo = now_paletteNo.Length >= 15 ? now_paletteNo.Substring(0, 15) : now_paletteNo;

                Now_Fabric.text = $"\n{now_barcode}\n\n\n{now_itemName}\n\n\n{now_paletteNo}";
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
