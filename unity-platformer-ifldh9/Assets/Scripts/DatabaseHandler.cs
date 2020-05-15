
using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
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
    public string nickname;

    public TMP_Text signUpResponseText;

    void onApplicationQuit()
    {
        if (con != null)
        {
            if (con.State.ToString() != "Closed")
            {
                con.Close();
            }
            con.Dispose();
        }
    }

    public string GetConnectionState()
    {
        return con.State.ToString();
    }

    [ClientRpc]
    private void RpcChangeName(string nickname)
    {
        Player player = GetComponent<Player>();
        player.text = nickname;
    }

    [TargetRpc]
    private void TargetSignUpFailed(NetworkConnection target)
    {
        RegistrationScreen registrationScreen = FindObjectOfType<RegistrationScreen>();
        registrationScreen.result.text = "Username is already taken or empty fields";
    }

    [TargetRpc]
    private void TargetSignUpSuccess(NetworkConnection target)
    {
        RegistrationScreen registrationScreen = FindObjectOfType<RegistrationScreen>();
        registrationScreen.result.text = "Success";
    }


    [TargetRpc]
    private void TargetLogin(NetworkConnection target, String nickname)
    {
        LoginScreen loginMenu = FindObjectOfType<LoginScreen>();
        loginMenu.inventoryCanvas.gameObject.SetActive(true);
        loginMenu.quickbarCanvas.gameObject.SetActive(true);
        loginMenu.loginCanvas.gameObject.SetActive(false);
        Player player = GetComponent<Player>();
        player.nameText.GetComponent<TMP_Text>().text = nickname;
    }

    [TargetRpc]
    private void TargetLoginFailed(NetworkConnection target)
    {
        LoginScreen loginMenu = FindObjectOfType<LoginScreen>();
        loginMenu.errorText.text = "wrong username or password";
    }

    [Command]
    public void CmdSignUp(String username, String password, String nickname)
    {
        if (username.Length != 0 && password.Length != 0 && nickname.Length != 0)
        {
            connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + this.password + ";CharSet=utf8;Pooling=";
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
                string sql = "SELECT username FROM ifldh9.users WHERE username= + '" + username + "';";
                cmd = new MySqlCommand(sql, con);

                rdr = cmd.ExecuteReader();
                if (rdr.HasRows)
                {
                    TargetSignUpFailed(connectionToClient);
                }
                else
                {
                    rdr.Close();
                    sql = "INSERT INTO `ifldh9`.`users` (`username`, `password`, `nickname`) VALUES ('" + username + "', '" + password + "', '" + nickname + "');";
                    cmd = new MySqlCommand(sql, con);
                    rdr = cmd.ExecuteReader();
                    TargetSignUpSuccess(connectionToClient);
                }

                rdr.Close();
                con.Close();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }
        else
        {
            TargetSignUpFailed(connectionToClient);
        }
    }

    [Command]
    public void CmdLogin(String username, String password)
    {
        connectionString = "Server=" + host + ";Database=" + database + ";User=" + user + ";Password=" + this.password + ";CharSet=utf8;Pooling=";
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

            string sql = "SELECT password,nickname FROM ifldh9.users WHERE username= + '" + username + "';";
            cmd = new MySqlCommand(sql, con);
            rdr = cmd.ExecuteReader();
            if (rdr.HasRows)
            {
                while (rdr.Read())
                {
                    if (rdr[0].ToString().Equals(password))
                    {
                        Player player = GetComponent<Player>();
                        player.text = rdr[1].ToString();
                        TargetLogin(connectionToClient, rdr[1].ToString());
                        RpcChangeName(rdr[1].ToString());
                    }
                    else
                    {
                        TargetLoginFailed(connectionToClient);
                    }
                }
            }
            else
            {
                TargetLoginFailed(connectionToClient);
            }

            rdr.Close();
            con.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }

    }
}
