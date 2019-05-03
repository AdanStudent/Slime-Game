using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerId : NetworkBehaviour
{
   /*[SyncVar]
    public string playerUniqueIdentity = "";
    Transform myTransform;
    private void Awake()
    {
        myTransform = transform;
    }
    void Update()
    {
        if (myTransform.name == "" || myTransform.name == "Temp Player2(Clone)")
        {
            myTransform.name = playerUniqueIdentity;
        }
    }

    public void SetIdentity(string playerName)
    {
        playerUniqueIdentity = playerName;
    }
    /*public string playerUniqueIdentity;
   // [SyncVar]
    int players = 1;
    private NetworkInstanceId playerNetID;
    private Transform myTransform;
    public override void OnStartLocalPlayer()
    { 
        base.OnStartLocalPlayer();
        if (isServer == true)
            CmdGetNetIdentity();
        else
            RpcGetNetIdentity();
        SetIdentity();
    }
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void Awake()
    {
        myTransform = transform;
    }
    // Update is called once per frame
    void Update()
    {
        if(myTransform.name == "" || myTransform.name == "Temp Player2(Clone)")
        {
            SetIdentity();
        }
    }
    [Command]
    void CmdGetNetIdentity()
    {
        RpcGetNetIdentity();
    }
    [ClientRpc]
    void RpcGetNetIdentity()
    {
        //playerNetID = GetComponent<NetworkIdentity>().netId;
        CmdTellServerMyIdentity(MakeUniqueIdentity());
    }

    string MakeUniqueIdentity()
    {
        string uniqueName = "Player " + players; //playerNetID.ToString();
        players++;
        return uniqueName;
    }

    void SetIdentity()
    {
        if (!isLocalPlayer)
        {
            myTransform.name = playerUniqueIdentity;
        }
        else
        {
            myTransform.name = MakeUniqueIdentity();
        }
    }


    [Command]
    void CmdTellServerMyIdentity(string name)
    {
        playerUniqueIdentity = name;
    }*/
}
