using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkDiscovery : NetworkDiscovery
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public void StartServer()
    {
        //NetworkManager.singleton.networkAddress = "192.168.1.255";
        NetworkManager.singleton.StartHost();
        broadcastData = "26000";
        Initialize();
        StartAsServer();
    }
}
