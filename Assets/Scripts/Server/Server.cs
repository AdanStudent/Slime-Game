﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkBehaviour
{
    public GameObject playerUnit;
    public GameObject spawnArea;
    // public GameObject spawnArea;
    // Start is called before the first frame update
    void Start()
    {
        NetworkManager.singleton.maxConnections = 4;
        if(isServer == true)
        {
            //SpawnArea();
        }
        if (isLocalPlayer == false)
        {
            return;
        }
        CmdSpawnPersonalPlayer();
        CmdSpawnArea();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    void CmdSpawnPersonalPlayer()
    {
        GameObject myPlayer = Instantiate(playerUnit);
        NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
        Debug.Log("Spawning Object");
    }

    [Command]
  void CmdSpawnArea()
    {
        GameObject SpawnArea = Instantiate(spawnArea);
        NetworkServer.Spawn(SpawnArea);
        Debug.Log("Spawn Area is Spawning");
    }



}
