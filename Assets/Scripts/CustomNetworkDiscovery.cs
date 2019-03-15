using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class CustomNetworkDiscovery : NetworkDiscovery
{

    bool ServerFound = false;
    bool startClient = false;
   
    void Start()
    {
        
    }

    public void StartClient()
    {
        Initialize();
        if(startClient == false)
        {

            StartAsClient();
            startClient = true;
        }
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if(ServerFound == false && startClient == true)
        {
            ServerFound = true;
            string ipAddress = fromAddress;
            string IP = ipAddress.Substring(6);
            NetworkManager.singleton.networkAddress = fromAddress;
            Debug.Log("Found ip address" + fromAddress);
            Debug.Log("corrected ip address" + IP);
            NetworkManager.singleton.StartClient();
        }


    }
  
}
