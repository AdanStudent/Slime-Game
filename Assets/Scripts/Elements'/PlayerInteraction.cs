using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteraction : NetworkBehaviour
{
    //player's element type
  
    public ElementEnum.Elements elementType = ElementEnum.Elements.Ash;
    //Object renderer
    private Renderer renderer1;
    //element materials
    public Material ash;
    public Material fire;
    public Material grass;
    public Material water;
    public Material cheese;
    private Server serverRef;
    public LivesStruct tempLives;

    // Start is called before the first frame update
    void Start()
    {
        GameObject server = GameObject.FindGameObjectWithTag("Server");
        serverRef = server.GetComponent<Server>();
        //initialize the render
        renderer1 = gameObject.GetComponent<Renderer>();
        //intialize material
        ChangeMaterial();
        //freeze rotation
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        
       
    }

    private void Awake()
    {
        tempLives = new LivesStruct(this.gameObject.GetComponent<NetworkIdentity>().netId.ToString(), 2);
        if (isServer == true)
            RpcUpdateStruct();
        else
            CmdUpdateStruct();
        
    }

    [Command]
    void CmdUpdateStruct()
    {
        RpcUpdateStruct();
    }

    [ClientRpc]
    void RpcUpdateStruct()
    {
        serverRef.playerLives.Add(tempLives);
    }

    private void callRespawn()
    {
        serverRef.myPlayer = this.gameObject;
        if (isServer == true)
            serverRef.RpcPlayerRespawn();
        else
            serverRef.CmdPlayerRespawn();
    }
    [Command]
    public void CmdSetType(ElementEnum.Elements elements)
    {
        RpcSetType(elements);
    }

    [ClientRpc]
    //Set the new element type for player if new element is picked up
    public void RpcSetType(ElementEnum.Elements element)
    {
        Debug.Log(this+" Current Type: " + elementType.ToString());
        elementType = element;
        ChangeMaterial();
        Debug.Log(this+" New Type: " + elementType.ToString());
    }

//     [ClientRpc]
//     //Spawn the player again
//     public void RpcSetActiveAgain()
//     {
//         gameObject.SetActive(true);
//     }

    //change the player's element type
    public void ChangeMaterial()
    {
        switch(elementType)
        {
            case ElementEnum.Elements.Ash:
                renderer1.material = ash;
                break;
            case ElementEnum.Elements.Cheese:
                renderer1.material = cheese;
                break;
            case ElementEnum.Elements.Fire:
                renderer1.material = fire;
                break;
            case ElementEnum.Elements.Grass:
                renderer1.material = grass;
                break;
            case ElementEnum.Elements.Water:
                renderer1.material = water;
                break;
            default:
                renderer1.material = ash;
                break;
        }
    }

    [ClientRpc]
    public void RpcComparePlayersElementTypes(GameObject other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        switch (elementType)
        {
            //check other player against current element
            case ElementEnum.Elements.Ash:
                //Destroy self if lose
                switch (interaction.elementType)
                {
                    //cheese always wins
                    case ElementEnum.Elements.Cheese:
                        Debug.Log(this + " Loses to cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                        //Grass beats ash
                    case ElementEnum.Elements.Grass:
                        Debug.Log(this + " Loses to Grass");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                }
                break;
                //cheese always win
            case ElementEnum.Elements.Cheese:
                Debug.Log("Cheese always wins");
                break;
            case ElementEnum.Elements.Fire:
                switch (interaction.elementType)
                {
                    //cheese always wins
                    case ElementEnum.Elements.Cheese:
                        Debug.Log(this + " Loses to Cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                        //water beats fire
                    case ElementEnum.Elements.Water:
                        Debug.Log(this + " Loses to Water");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                }
                break;

            case ElementEnum.Elements.Water:
                switch (interaction.elementType)
                {
                    //ahs beats water
                    case ElementEnum.Elements.Ash:
                        Debug.Log(this + " Loses to Ash");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                    case ElementEnum.Elements.Cheese:
                        Debug.Log(this + " Loses to Cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                }
                break;
            case ElementEnum.Elements.Grass:
                switch (interaction.elementType)
                {
                    case ElementEnum.Elements.Cheese:
                        Debug.Log(this + " Loses to Cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                        //fire beats grass
                    case ElementEnum.Elements.Fire:
                        Debug.Log(this + " Loses to Fire");
                        callRespawn();
                        gameObject.SetActive(false);
                        break;
                }
                break;
        }
    }


    [Command]
    void CmdComparePlayersElementTypes(GameObject other)
    {
        RpcComparePlayersElementTypes(other);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            if (isServer == true)
                RpcComparePlayersElementTypes(other.gameObject);
            else
                CmdComparePlayersElementTypes(other.gameObject);
        }
    }
}
