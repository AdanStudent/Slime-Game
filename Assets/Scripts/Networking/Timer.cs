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

    bool gameOver = false;
    Timer serverTimer;
    GUIStyle style = new GUIStyle();
    public AudioClip fightMusic;
 
    void Start()
    {
        Timer[] timers = FindObjectsOfType<Timer>();
        style.fontSize = 25;
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        //if (timers.Length > 0)
        {
            for (int i = 0; i < timers.Length; i++)
            {
                if (i == timers.Length-1)
                {
                    serverTimer = this;
                    masterTimer = true;
                    timer = gameTime;
                    AudioSource source = GetComponent<AudioSource>();
                    source.loop = true;
                    source.clip= fightMusic;
                    source.Play();
                }
                else
                {
                    serverTimer = timers [i];
                    break;
                }
            }
        }

    }

    void OnGUI()
    {
        if (masterTimer)
        {
            GUI.Label(new Rect(10, 10, 100, 20), $"Timer:{Mathf.RoundToInt(timer)}",style);
        }
    }

    void Update()
    {
        if(masterTimer)
        { //Only the MASTER timer controls the time
            if(timer < 0 && timer > -2)
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
                if (!gameOver)
                {
                    gameOver = true;
                    StartCoroutine(WaitUntilLobby());
                }
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

    [Command]
    public void CmdReturnToLobby()
    {
        LobbyManager.s_Singleton.SendReturnToLobby();
    }

    IEnumerator WaitUntilLobby()
    {
        yield return new WaitForSeconds(2);
        CmdReturnToLobby();

    }

}
