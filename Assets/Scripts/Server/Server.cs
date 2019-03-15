using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server : NetworkBehaviour
{
    public GameObject playerUnit;
    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer == false)
        {
            return;
        }
        CmdSpawnPersonalPlayer();
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

   
    


}
