using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Text;
using System.Net.Sockets;


public class CustomNetworkManager : NetworkManager
{
    public ServerNetworkDiscovery SND;
    public CustomNetworkDiscovery CND;
        //Chekcking to see if at menu
        // public bool isatStartup = true;
        public const int PORT = 26000;
    private int lastInt = 0;
    private bool foundServer = false;
    ConnectionConfig connection;
    private const int MAX_CONNECTIONS = 4;

    private const int SERVERPORT = 5000;
    private string serverIP;

    private const int STARTPORT = 5100;
    private IPAddress groupAddress = IPAddress.Parse("127.0.0.1");
    private UdpClient udpClient;
    private IPEndPoint remoteEnd;

    

    //this cimputers client
    NetworkClient myClient;
    bool ConnectedToServer = false;


    private void Start()
    {
    
       // connection = new ConnectionConfig();
       // connection.AddChannel(QosType.Reliable);
    }
    private void Update()
    {
//           if(myClient != null)
//         {
//             if(myClient.isConnected == true)
//             {
// 
//                 //SetIPAddress();
//             }
//             else
//             {
//                 myClient.Disconnect();
//                 myClient.Connect("192.168.1.255", PORT);
//             }
//         }
    }
    public void StartupHost()
    {
        SetPort();
        SND.StartServer();

    }

    public void JoinGame()
    {
        
        SetPort();
        CND.StartClient();
       

    }
 


    void SetIPAddress()
    {
        if (lastInt < 256)
        {
            string IPAddressTest = "192.168.1." + lastInt;
          
            Debug.Log("Current IP Address: " + IPAddressTest);
            lastInt++;
        }

        //         // string ipAddress = GameObject.Find("InputFieldIPAddress").transform.Find("Text").GetComponent<Text>().text;
        //         if (lastInt < 256)
        //         {
        //             lastInt++;
        //             string IPAddressTest = "192.168.1." + lastInt;
        //             NetworkManager.singleton.networkAddress = IPAddressTest;
        //             Debug.Log("Current IP Address: " + IPAddressTest);
        //         }

        //         
    }

    private void SetPort()
    {
        NetworkManager.singleton.networkPort = PORT;
    }
    private void SetServerPort()
    {
        NetworkManager.singleton.networkPort = SERVERPORT;
        //StartCoroutine(StartBroadcast());

    }




    private void OnLevelWasLoaded(int level)
    {
        if(level == 0)
        {
            SetupMenuSceneButtons();
        }
        else
        {
           // foundServer = false;
            SetupOtherSceneButtons();
        }
    }

    void SetupMenuSceneButtons()
    {
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonStartHost").GetComponent<Button>().onClick.AddListener(StartupHost);

        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("ButtonJoinGame").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }

    void SetupOtherSceneButtons()
    {
        //GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.RemoveAllListeners();
        //GameObject.Find("ButtonDisconnect").GetComponent<Button>().onClick.AddListener(NetworkManager.singleton.StopHost);
    }
    
}
