using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//based on code from this site
//https://answers.unity.com/questions/1179602/implementing-server-side-code-with-unet.html

public class Timer : NetworkBehaviour
{
    [SyncVar] public float gameTime; //The length of a game, in seconds.
    [SyncVar] public float timer = -1; //How long the game has been running. -1=waiting for players, -2=game is done
    [SyncVar] public int minPlayers; //Number of players required for the game to start
    [SyncVar] public bool masterTimer = false; //Is this the master timer?

    [SyncVar] public float masterDeltaTime;

    Timer serverTimer;

    private LobbyManager lobby;
 
    void Start()
    {
        lobby = GameObject.FindGameObjectWithTag("Lobby").GetComponent<LobbyManager>();

        print(lobby.isAHost.ToString());

        if(!lobby.isAHost)
        {
            Timer[] timers = FindObjectsOfType<Timer>();
            for(int i =0; i < timers.Length; i++)
            {
                if(timers[i].masterTimer)
                {
                    serverTimer = timers [i];
                }
            }
        }
        else
        {
            serverTimer = this;
            masterTimer = true;
            timer = gameTime;
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), $"Timer:{Mathf.RoundToInt(timer)}");
    }

    void Update()
    {
        if(masterTimer)
        { //Only the MASTER timer controls the time
            if(timer < 0)
            {
                timer = -2;
            }
            else if(timer == -1)
            {
                if(NetworkServer.connections.Count >= minPlayers)
                {
                    timer = 0;
                }
            }
            else if(timer == -2)
            {
                //Game done.
            }
            else
            {
                masterDeltaTime = Time.deltaTime;
                timer -= masterDeltaTime;
            }
        }


        if (serverTimer)
        {
            gameTime = serverTimer.gameTime;
            timer = serverTimer.timer;
            minPlayers = serverTimer.minPlayers;
        }
        else
        {
            Timer[] timers = FindObjectsOfType<Timer>();

            for(int i =0; i < timers.Length; i++)
            {
                if(timers[i].masterTimer)
                {
                    serverTimer = timers[i];
                }
            }
        }
    }

 }
