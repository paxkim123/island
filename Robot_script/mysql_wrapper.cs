using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using MySql.Data.MySqlClient; // MariaDB/MySQL 관련 네임스페이스
using UnityEngine.UI;
using UnityEngine;

public class mysql_wrapper
{
    private MySqlConnection _sqlConnection;
    private MySqlTransaction _sqlTransaction;

    private string Server_IP;
    private string Server_Port;
    private string Server_DB_NAME;
    private string Server_ID;
    private string Server_PW;

    private string PREFIX;

    //한글한글
    public mysql_wrapper(string Server = "", string prefix = "")
    {
        INI_wrapper ini = new INI_wrapper(Server);

        PREFIX = prefix;
        if (PREFIX != "") PREFIX += "_";

        Server_IP = ini.read(PREFIX + "IP");
        Server_Port = ini.read(PREFIX + "PORT");
        Server_DB_NAME = ini.read(PREFIX + "DB");
        Server_ID = ini.read(PREFIX + "ID");
        Server_PW = ini.read(PREFIX + "PW");

        _sqlConnection = new MySqlConnection();
    }

    public bool Connect()
    {
        return Connect(Server_IP, Server_Port, Server_DB_NAME, Server_ID, Server_PW);
    }

    public void Disconnect()
    {
        try
        {
            if (_sqlConnection == null) return;

            // Disconnect
            _sqlConnection.Close();
        }
        catch
        {
            _sqlConnection = null;
        }
    }

    public bool Connect(string ip, string port, string name, string uid, string pwd)
    {
        bool bRet = false;

        try
        {
            string connectionString = $"Server={ip};Port={port};"
                                    + $"Database={name};"
                                    + $"Uid={uid};"
                                    + $"Pwd={pwd};";

            _sqlConnection = new MySqlConnection();

            // Connect to the database
            _sqlConnection.ConnectionString = connectionString;
            _sqlConnection.Open();

            bRet = _sqlConnection.State.Equals(ConnectionState.Open);
        }
        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            bRet = false;
        }

        return bRet;
    }

    public DataTable Select(string sql)
    {
        try
        {
            DataTable dataTable = new DataTable();

            MySqlDataAdapter sqlDataAdapter = new MySqlDataAdapter(sql, _sqlConnection);
            sqlDataAdapter.SelectCommand.CommandTimeout = 500;

            DataSet dataSet = new DataSet();

            sqlDataAdapter.Fill(dataSet);

            if (dataSet.Tables.Count > 0)
                dataTable = dataSet.Tables[0];

            return dataTable;
        }
        catch
        {
            throw;
        }
    }

    public bool OnInsertOrUpdateRequest(string str_query)
    {
        try
        {
            // 연결 상태 확인
            if (_sqlConnection.State != ConnectionState.Open)
            {
                // 연결이 닫혀 있으면 다시 연결
                Connect();
            }

            MySqlCommand sqlCommand = new MySqlCommand();
            sqlCommand.Connection = _sqlConnection;
            sqlCommand.CommandText = str_query;
            sqlCommand.ExecuteNonQuery();
            return true;
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }
    }

    public bool BeginTransaction()
    {
        try
        {
            _sqlTransaction = _sqlConnection.BeginTransaction();
            return true;
        }
        catch
        {
            throw;
        }
    }

    public int TransactionNonQuery(string sql, string[] pKey = null, object[] pValue = null)
    {
        try
        {
            MySqlCommand sqlCommand = new MySqlCommand(sql, _sqlConnection, _sqlTransaction);
            if (pKey != null)
            {
                for (int i = 0; i < pKey.Length; i++)
                {
                    sqlCommand.Parameters.AddWithValue(pKey[i], pValue[i]);
                }
            }
            int r = sqlCommand.ExecuteNonQuery();
            return r;
        }
        catch
        {
            throw;
        }
    }

    public bool Commit()
    {
        try
        {
            _sqlTransaction.Commit();
            return true;
        }
        catch
        {
            throw;
        }
    }

    public bool Rollback()
    {
        try
        {
            _sqlTransaction.Rollback();
            return true;
        }
        catch
        {
            throw;
        }
    }
}
