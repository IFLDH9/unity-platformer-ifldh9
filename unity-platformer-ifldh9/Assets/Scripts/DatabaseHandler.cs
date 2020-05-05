
using UnityEngine;
using System;
using System.Data;
using System.Text;

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

using MySql.Data;
using MySql.Data.MySqlClient;
using System.Net;
using UnityEngine.Networking;

public class DatabaseHandler : NetworkBehaviour
{
    public string host, database, user, password;
    public bool pooling = true;

    private string connectionString;
    private MySqlConnection con = null;
    private MySqlCommand cmd = null;
    private MySqlDataReader rdr = null;

    private MD5 _md5Hash;
    public Canvas loginScreen;

    void Awake()
    {
      //  DontDestroyOnLoad(this.gameObject);
    }
    void onApplicationQuit()
    {
        if (con != null)
        {
            if (con.State.ToString() != "Closed")
            {
                con.Close();
                Debug.Log("Mysql connection closed");
            }
            con.Dispose();
        }
    }

    public string GetConnectionState()
    {
        return con.State.ToString();
    }

    public void Login(String username,String password)
    {
        if(isClient)
        {
            CmdLogin(username,password);
        }
    }


    [TargetRpc]
    public void TargetLogin(NetworkConnection target)
    {
        Debug.Log("eljutottam :) ");
        loginScreen.gameObject.SetActive(!loginScreen.gameObject.activeSelf);
        // loginScreen.login = true;
        //loginScreen.mainMenu.gameObject.SetActive(false);
    }

    [Command]
    public void CmdLogin(String username,String password)
    {
        Debug.Log("loginelek");
        connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + password + ";CharSet=utf8;Pooling=";
        if (pooling)
        {
            connectionString += "True";
        }
        else
        {
            connectionString += "False";
        }
        try
        {
            con = new MySqlConnection(connectionString);
            con.Open();
            Debug.Log("Mysql state: " + con.State);

            string sql = "SELECT password FROM ifldh9.users WHERE username= + '" + username + "';";
            cmd = new MySqlCommand(sql, con);
            //			string sql = "SELECT * FROM clothes";
            //			cmd = new MySqlCommand(sql, con);
            rdr = cmd.ExecuteReader();
            //
            while (rdr.Read())
            {
                Debug.Log("???");
                Debug.Log(rdr[0].ToString() + " hello " + password);
                Debug.Log(rdr[0].ToString().Equals(password));
                if (rdr[0].ToString().Equals(password))
                {
                    Debug.Log("helyes");
                    TargetLogin(connectionToClient);
                }
            }
            rdr.Close();

        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
