using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkBehaviour
{
    public GameObject playerUnit;
    public GameObject spawnArea;
    public List<Transform> spawnPoints;
    // public GameObject spawnArea;
    // Start is called before the first frame update
    void Start()
    {
        if(isServer == true)
        {
            //SpawnArea();
        }
        if (isLocalPlayer == false)
        {
            return;
        }
        CmdSpawnArea();
        CmdSpawnPersonalPlayer();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    [Command]
    void CmdSpawnPersonalPlayer()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(0,spawnPoints.Count);
        GameObject myPlayer = Instantiate(playerUnit,spawnPoints[index].position,spawnPoints[index].rotation);
        NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
        Debug.Log("Spawning Object");
    }

    [Command]
    void CmdSpawnArea()
    {
        GameObject spawn = GameObject.FindGameObjectWithTag("SpawnArea");
        if (spawn == null)
        {
            GameObject SpawnArea = Instantiate(spawnArea);
            NetworkServer.Spawn(SpawnArea);
            Debug.Log("Spawn Area is Spawning");
        }
    }



}
