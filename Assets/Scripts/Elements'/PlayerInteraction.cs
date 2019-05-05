using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerInteraction : NetworkBehaviour
{
    //player's element type
  
    public ElementEnum.Elements elementType = ElementEnum.Elements.Ash;
    public ElementEnum.Elements previousElementType = ElementEnum.Elements.Ash;
    //Object renderer
    private Renderer renderer1;
    //element materials
    public Material ash;
    public Material fire;
    public Material grass;
    public Material water;
    public Material cheese;
    private Server serverRef;
    private int lives;
    public bool Respawning = false;

    public LivesStruct tempLives;
    public float cheeseTime=7;
    public string Winner = "";
    private bool iWon = false;
    private bool someoneWon;

    public AudioClip Dying;

    // Start is called before the first frame update
    void Start()
    {
        lives = 3;
        Respawning = false;
        GameObject server = GameObject.FindGameObjectWithTag("Server");
        System.Random rnd = new System.Random(System.Guid.NewGuid().GetHashCode());
        int randomType = rnd.Next(0, 3);
        switch(randomType)
        {
            case 0:
                elementType = ElementEnum.Elements.Fire;
                break;
            case 1:
                elementType = ElementEnum.Elements.Water;
                break;
            case 2:
                elementType = ElementEnum.Elements.Grass;
                break;
        }

        serverRef = server.GetComponent<Server>();
        //initialize the render
        renderer1 = gameObject.GetComponent<Renderer>();
        if (isServer == true)
            RpcSetType(elementType);
        else
            CmdSetType(elementType);

        //freeze rotation
        gameObject.GetComponent<Rigidbody>().freezeRotation = true;
        tempLives = new LivesStruct(this.gameObject.GetComponent<NetworkIdentity>().netId.ToString(), 3, this.gameObject);
        serverRef.playerLives.Add(tempLives);



    }

    private void Awake()
    {
        if (isServer)
        {
            this.gameObject.name = "LocalPlayer";
        }
        else
        {
            this.gameObject.name = "RemotePlayer";
        }

        /* GameObject server = GameObject.FindGameObjectWithTag("Server");
         serverRef = server.GetComponent<Server>();
         tempLives = new LivesStruct(this.gameObject.GetComponent<NetworkIdentity>().netId.ToString(), 3);
         serverRef.playerLives.Add(tempLives);*/
    }
    private void OnGUI()
    {
        if (hasAuthority == true)
        {
            GUI.Label(new Rect(450, 10, 100, 20), $"Lives:{lives}");
            
        }
        
    }

    public void SetWinner(string netID)
    {
        Winner = netID;
        iWon = CheckIfIWon();
        Debug.Log("SetWinner is Called");

    }

    private bool CheckIfIWon()
    {
        if (Winner == this.gameObject.GetComponent<NetworkIdentity>().netId.ToString())
        {
            return true;
        }
        else return false;
    }
    private void RespawnDelay()
    {
        Respawning = false;
    }

    private void UpdateLives()
    {
        lives--;
        //GameObject server = GameObject.FindGameObjectWithTag("Server");
        //serverRef = server.GetComponent<Server>();
       /* int currentLives = lives;
        for (int i = 0; i < serverRef.playerLives.Count; i++)
        {pdatel
            if (this.gameObject.GetComponent<NetworkIdentity>().netId.ToString()  == serverRef.playerLives[i].netID)
            {
                currentLives = serverRef.playerLives[i].lives;
                --;
            }
        }
        lives = currentLives;*/
    }
    float timePassed = 0;
    private void Update()
    {
        //cheese is temporary
        if (elementType==ElementEnum.Elements.Cheese)
        {
            //check timer
            if(timePassed<cheeseTime)
            {
                timePassed += Time.deltaTime;
            }
            //if cheese time has passed then reset element
            else
            {
                if (isServer == true)
                    RpcSetType(previousElementType);
                else
                    CmdSetType(previousElementType);
                timePassed = 0;
            }
        }
    }
    private void callRespawn()
    {
        if (Respawning == false)
        {
            
            Respawning = true;
            Invoke("RespawnDelay", 4.0f);
            serverRef.RespawnReference(this.gameObject);
            UpdateLives();
            if (isServer == true)
                serverRef.RpcPlayerRespawn();
            else
                serverRef.CmdPlayerRespawn();
        }
    }
    //server command
    [Command]
    public void CmdSetType(ElementEnum.Elements elements)
    {
        RpcSetType(elements);
    }

    [ClientRpc]
    //Set the new element type for player if new element is picked up
    public void RpcSetType(ElementEnum.Elements element)
    {
        //cheese is temporary so store previous type
        if (element == ElementEnum.Elements.Cheese)
            previousElementType = elementType;
        //change material
        //Debug.Log(this+" Current Type: " + elementType.ToString());
        elementType = element;
        ChangeMaterial();
        //Debug.Log(this+" New Type: " + elementType.ToString());
    }



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

    AudioSource source;

    [ClientRpc]
    public void RpcComparePlayersElementTypes(GameObject other)
    {
        PlayerInteraction interaction = other.GetComponent<PlayerInteraction>();
        switch (elementType)
        {
            ////check other player against current element
            //case ElementEnum.Elements.Ash:
            //    //Destroy self if lose
            //    switch (interaction.elementType)
            //    {
            //        //cheese always wins
            //        case ElementEnum.Elements.Cheese:
            //            Debug.Log(this + " Loses to cheese");
            //            callRespawn();
            //            gameObject.SetActive(false);
            //            break;
            //            //Grass beats ash
            //        case ElementEnum.Elements.Grass:
            //            Debug.Log(this + " Loses to Grass");
            //            callRespawn();
            //            gameObject.SetActive(false);
            //            break;
            //    }
            //    break;
            //cheese always win
            case ElementEnum.Elements.Cheese:
                Debug.Log("Cheese always wins");
                DyingSound(other);
                break;
            case ElementEnum.Elements.Fire:
                switch (interaction.elementType)
                {
                    //cheese always wins
                    case ElementEnum.Elements.Cheese:
                      //  Debug.Log(this + " Loses to Cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        DyingSound(other);
                        break;
                        //water beats fire
                    case ElementEnum.Elements.Water:
                     //   Debug.Log(this + " Loses to Water");
                        callRespawn();
                        gameObject.SetActive(false);
                        DyingSound(other);
                        break;
                }
                break;

            case ElementEnum.Elements.Water:
                switch (interaction.elementType)
                {
                    //ahs beats water
                    case ElementEnum.Elements.Grass:
                        Debug.Log(this + " Loses to Ash");
                        callRespawn();
                        gameObject.SetActive(false);
                        DyingSound(other);
                        break;
                    case ElementEnum.Elements.Cheese:
                       // Debug.Log(this + " Loses to Cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        DyingSound(other);
                        break;
                }
                break;
            case ElementEnum.Elements.Grass:
                switch (interaction.elementType)
                {
                    case ElementEnum.Elements.Cheese:
                       // Debug.Log(this + " Loses to Cheese");
                        callRespawn();
                        gameObject.SetActive(false);
                        DyingSound(other);
                        break;
                        //fire beats grass
                    case ElementEnum.Elements.Fire:
                       // Debug.Log(this + " Loses to Fire");
                        callRespawn();
                        gameObject.SetActive(false);
                        DyingSound(other);
                        break;
                }
                break;
        }
    }

    private void DyingSound(GameObject other)
    {
        source = other.GetComponent<AudioSource>();
        source.PlayOneShot(Dying);
    }


    //sever command
    [Command]
    void CmdComparePlayersElementTypes(GameObject other)
    {
        RpcComparePlayersElementTypes(other);
    }
    //compare element types
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
public class Delay
{
    public float WaitTime;
    private float completionTime;

    public Delay(float waitTime)
    {
        WaitTime = waitTime;
        Reset();
    }

    public void Reset()
    {
        completionTime = Time.time + WaitTime;
    }

    public bool IsReady { get { return Time.time >= completionTime; } }
}