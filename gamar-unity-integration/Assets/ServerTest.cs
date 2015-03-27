using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using GamAR.Networking;
using GamAR.Networking.Responses;
using GamAR;
using UnityEditor;

public class ServerTest : MonoBehaviour
{
    public string ServerURL = "http://local.loop/shared/gameserver/1.0/";
    private Server server;

    private string note = "";

    private string loginName = "";
    private string loginDeviceId = "";

    private string registerEmail = "";
    private string registerPass = "";
    private string registerName = "";
    private string registerCharacter = "";
    private string deviceId = "";
    private string groupCode = "";

    private int markerId = 0;
    private string markerValue = "";

    private string gameScore = "0";

    void Start() 
    {
        server = new Server(ServerURL, this);
	}

    void OnGUI()
    {
        float centerX = 0.5f * Screen.width - 100f;

        float posX = centerX;
        float posY = 50;

        if (server.Loading)
        {
            GUI.Label(new Rect(posX, posY, 200, 20), "Loading...");
            return;
        }

        if (server.Connected)
        {
            if (GUI.Button(new Rect(posX - 14, posY, 228, 24), "LOGOUT"))
            {
                Debug.Log("LOGOUT CLICKED");
                server.Logout(logoutResponse);
            }

            posY += 34;

            if (server.CurrentCheckin != null) // playing the game ...
            {
                string description = string.Format("Game: '{0}', Tries: {1}\nAssets URL: '{2}'", server.CurrentCheckin.GameName, server.CurrentCheckin.Tries, server.CurrentCheckin.AssetsUrl);

                GUI.Box(new Rect(posX - 14, posY, 228, 170 + server.MarkerNames.Length * 20), "GAME RESULTS"); posY += 30;
                GUI.Label(new Rect(posX, posY, 200, 48), description); posY += 50;
                GUI.Label(new Rect(posX, posY, 200, 20), "Score (0-100)"); posY += 20;

                string score = GUI.TextField(new Rect(posX, posY, 200, 20), gameScore, 25); posY += 36;
                int scoreValue = 0;

                if (int.TryParse(score, out scoreValue))
                {
                    if (scoreValue > 100)
                    {
                        gameScore = "100";
                        scoreValue = 100;
                    }
                    else if (scoreValue < 0)
                    {
                        gameScore = "0";
                        scoreValue = 0;
                    }
                    else gameScore = score;

                    if (GUI.Button(new Rect(posX, posY, 90, 20), "SUBMIT")) server.Score(scoreValue, scoreResponse);
                    if (GUI.Button(new Rect(posX + 100, posY, 90, 20), "CANCEL")) server.Checkout(checkoutResponse);
                }
            }
            else
            {
                // game check-in
                GUI.Box(new Rect(posX - 14, posY, 228, 170 + server.MarkerNames.Length * 20), "GAME CHECK-IN"); posY += 30;

                GUI.Label(new Rect(posX, posY, 200, 20), "Marker"); posY += 24;
                GUILayout.BeginArea(new Rect(posX + 10, posY, 180, server.MarkerNames.Length * 24));
                markerId = GUILayout.SelectionGrid(markerId, server.MarkerNames, 1, EditorStyles.radioButton);
                GUILayout.EndArea();

                posY += server.MarkerNames.Length * 20 + 20;

                GUI.Label(new Rect(posX, posY, 200, 20), "Value"); posY += 20;
                markerValue = GUI.TextField(new Rect(posX, posY, 200, 20), markerValue, 25); posY += 36;

                if (GUI.Button(new Rect(posX, posY, 200, 20), "CHECK-IN")) server.Checkin(markerId, markerValue, checkinResponse);
            }
        }
        else // login / register UI
        {
            // login box
            GUI.Box(new Rect(posX - 14, posY, 228, 170), "LOGIN"); posY += 30;
            /*
            GUI.Label(new Rect(posX, posY, 200, 20), "E-Mail Address"); posY += 20;
            loginEmail = GUI.TextField(new Rect(posX, posY, 200, 20), loginEmail, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Password"); posY += 20;
            loginPass = GUI.TextField(new Rect(posX, posY, 200, 20), loginPass, 25); posY += 36;
            */

            GUI.Label(new Rect(posX, posY, 200, 20), "Nickname"); posY += 20;
            loginName = GUI.TextField(new Rect(posX, posY, 200, 20), loginName, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Device ID"); posY += 20;
            loginDeviceId = GUI.TextField(new Rect(posX, posY, 200, 20), loginDeviceId, 25); posY += 36;

            if (GUI.Button(new Rect(posX, posY, 200, 20), "SUBMIT")) server.Login(loginName, loginDeviceId, loginResponse);

            // register box
            posY += 50;
            GUI.Box(new Rect(posX - 14, posY, 228, 370), "REGISTER"); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Group Code*"); posY += 20;
            groupCode = GUI.TextField(new Rect(posX, posY, 200, 20), groupCode, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Name*"); posY += 20;
            registerName = GUI.TextField(new Rect(posX, posY, 200, 20), registerName, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Character*"); posY += 20;
            registerCharacter = GUI.TextField(new Rect(posX, posY, 200, 20), registerCharacter, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "E-Mail Address"); posY += 20;
            registerEmail = GUI.TextField(new Rect(posX, posY, 200, 20), registerEmail, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Password (at least 4 characters)"); posY += 20;
            registerPass = GUI.TextField(new Rect(posX, posY, 200, 20), registerPass, 25); posY += 30;
            GUI.Label(new Rect(posX, posY, 200, 20), "Device ID*"); posY += 20;
            deviceId = GUI.TextField(new Rect(posX, posY, 200, 20), deviceId, 25); posY += 36;

            if (GUI.Button(new Rect(posX, posY, 200, 20), "SUBMIT")) server.Register(groupCode, registerName, registerCharacter, registerEmail, registerPass, deviceId, registerResponse);
        }

        // show server error text
        if (server.ErrorId != 0)
        {
            GUI.Box(new Rect(centerX + 230, 50, 240, 40), "");
            GUI.contentColor = Color.red;
            GUI.Label(new Rect(centerX + 240, 60, 180, 20), server.ErrorText + " (" + server.ErrorId + ")");
        }
        else if (note != "")
        {
            GUI.contentColor = Color.black;
            GUI.Label(new Rect(posX + 240, posY, 200, 100), note);
        }
    }

    private void scoreResponse(ScoreResponse res)
    {
        if (res.Ready)
        {
            Debug.Log("SCORE: ok");
            note = string.Format("Time: {0}\nRank: {1}", res.Time, res.Rank);
        }
        else
        {
            Debug.Log("SCORE: " + res.ErrorText + "(" + res.ErrorId + ")");
        }
    }

    private void checkoutResponse(CheckoutResponse res)
    {
        if (res.Ready)
        {
            Debug.Log("CHECKOUT: ok");
            note = "";
        }
        else
        {
            Debug.Log("CHECKOUT: " + res.ErrorText + "(" + res.ErrorId + ")");
        }
    }

    private void checkinResponse(CheckinResponse res)
    {
        if (res.Ready)
        {
            Debug.Log("CHECKIN: ok");
            //note = string.Format("Game: {0}\nTries: {1}\nAssets URL: {2}", res.GameName, res.Tries, res.AssetsUrl);
        }
        else
        {
            Debug.Log("CHECKIN: " + res.ErrorText + "(" + res.ErrorId + ")");
        }
    }

    private void registerResponse(User u)
    {
        if (u.Validated)
        {
            Debug.Log("REGISTER: ok");
        }
        else
        {
            Debug.Log("REGISTER: " + server.ErrorText + "(" + server.ErrorId + ")");
        }
    }

    private void loginResponse(User u)
    {
        if (u.Validated)
        {
            Debug.Log("LOGIN: ok");
        }
        else
        {
            Debug.LogError("LOGIN: " + server.ErrorText + "(" + server.ErrorId + ")");
        }
    }

    private void logoutResponse(User u)
    {
        if (u.Validated)
        {
            Debug.LogError("LOGOUT FAILED!");
        }
        else
        {
            Debug.Log("LOGOUT DONE");
            note = "";
        }
    }

	void Update() 
    {
	
	}
}
