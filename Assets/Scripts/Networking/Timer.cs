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
 
    Timer serverTimer;
 
    void Start()
    {
        if(NetworkServer.connections.Count > 1)
        {
            Timer[] timers = FindObjectsOfType<Timer>();
            for(int i =0; i<timers.Length; i++)
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
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 20), $"Timer:{timer}");
    }

    void Update()
    {
        if(masterTimer)
        { //Only the MASTER timer controls the time
            if(timer>=gameTime)
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
                timer += Time.deltaTime;
            }
        }


        if (serverTimer)
        {
            gameTime = serverTimer.gameTime;
            timer = serverTimer.timer;
            minPlayers = serverTimer.minPlayers;
        }
        else
        { //Maybe we don't have it yet?
            Timer[] timers = FindObjectsOfType<Timer>();

            for(int i =0; i<timers.Length; i++)
            {
                if(timers[i].masterTimer)
                {
                    serverTimer = timers [i];
                }
            }
        }
    }

 }
