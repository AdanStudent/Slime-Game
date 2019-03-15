using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Server2 : NetworkManager
{
    private byte reliableChannel;
    private const int MAX_USER = 4;
    //private const int PORT = 26000;
    //private const int WEBPORT = 26001;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    /*public void Init()
    {
        NetworkTransport.Init();
        ConnectionConfig cc = new ConnectionConfig();
        //Channel: Road in which information travels
        //AddChannel returns a bite so that you know which channel it is 
        //ReliableEvery action is guaranteed to be delivered but no guarantee in order. 
        cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        //Server only code
        NetworkTransport.AddHost(topo, PORT, null);
        NetworkTransport.AddWebsocketHost(topo, WEBPORT, null);
    }*/
}
