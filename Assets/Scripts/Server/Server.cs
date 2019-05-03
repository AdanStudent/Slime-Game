using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;


public struct ElementStruct
{
    public int elementType;
    public Vector3 position;
}
public struct LivesStruct
{
    public string netID;
    public int lives;
    public LivesStruct(string net, int li)
    {
        netID = net;
        lives = li;
    }
    //GameObject 
}
public class SyncListLives : SyncListStruct<LivesStruct> { }
public class SyncListElement : SyncListStruct<ElementStruct> { }


public class Server : NetworkBehaviour
{
    public GameObject playerUnit;
    public GameObject spawnArea;
    //[SyncVar]
    public GameObject potionAsh;
    //[SyncVar]
    public GameObject potionFire;
   // [SyncVar]
    public GameObject potionGrass;
    //[SyncVar]
    public GameObject potionWater;
   // [SyncVar]
    public GameObject potionCheese;

    public GameObject Timer;

    public GameObject currentElement;
    public GameObject elementWheel;
    public GameObject lifeBar;

    public List<Transform> spawnPoints;
    //list of elements in the scene
    [SyncVar]
    public SyncListElement elementList = new SyncListElement();
    public GameObject myPlayer;
    private GameObject SpawnArea;
    private ElementSpawn elementSpawnRef;
    public SyncListLives playerLives = new SyncListLives();
    bool duplicateCheck = false;
    // public GameObject spawnArea;
    // Start is called before the first frame update
    public SyncListInt spawnPointIndexs = new SyncListInt();

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
        
        CmdSpawnArea();
        CmdSpawnPersonalPlayer();
        CmdSpawnTimer();
    }

    void CheckForDuplicates()
    {
        for (int i = 0; i < playerLives.Count; i++)
        {

            for (int j = 0; j < playerLives.Count; j++)
            {
                if((i!=j) && (playerLives[i].netID == playerLives[j].netID))
                {
                    playerLives.RemoveAt(j);
                }
            }
        }
        duplicateCheck = true;
    }
    [Command]
    void CmdSpawnPersonalPlayer()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(0, spawnPoints.Count);
        if (connectionToClient.isReady)
        {
            if (spawnPointIndexs.Contains(index))
            {
                for(int i=0;i<spawnPoints.Count;i++)
                {
                    if(!spawnPointIndexs.Contains(i))
                    {
                        index = i;
                        break;
                    }
                }
            }

            myPlayer = Instantiate(playerUnit, spawnPoints[index].position, spawnPoints[index].rotation);
            NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
            CmdSpawnLocalUI();
            spawnPointIndexs.Add(index);
        }
        else
        {
            //connectionToClient.RegisterHandler(MsgType.Ready, OnReady);
            StartCoroutine(WaitForReady());
        }
    }

    IEnumerator WaitForReady()
    {
        while (!connectionToClient.isReady)
        {
            yield return new WaitForSeconds(0.25f);
        }
        OnReady();
    }

    [Server]
    private void OnReady()
    {
        Debug.Log("Spawning Object1");
        System.Random rnd = new System.Random();
        int index = rnd.Next(0, spawnPoints.Count);
        if (connectionToClient.isReady)
        {
            if (spawnPointIndexs.Contains(index))
            {
                for (int i = 0; i < spawnPoints.Count; i++)
                {
                    if (!spawnPointIndexs.Contains(i))
                    {
                        index = i;
                        break;
                    }
                }
            }

            myPlayer = Instantiate(playerUnit, spawnPoints[index].position, spawnPoints[index].rotation);
            NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
            CmdSpawnLocalUI();

        }
    }

    //adds each player to dictonary
    /*[ClientRpc]
    void RpcAddPlayersToDictonary(string playerName)
    {
        playerLives.Add(playerName, 3);
        foreach (KeyValuePair<string, int> pl in playerLives)
        {
            Debug.Log("Player: " + pl.Key + " Lives: " + pl.Value);
        }
    }*/
    [Command]
    public void CmdSpawnLocalUI()
    {
        //current 
        GameObject current = Instantiate(currentElement);
        current.transform.rotation = Camera.main.transform.rotation;
        current.transform.position = new Vector3(Camera.main.transform.position.x + 1.05f,
   Camera.main.transform.position.y - 0.46f,
   Camera.main.transform.position.z + 0.84f);
        current.transform.parent = Camera.main.transform;
        NetworkServer.Spawn(current);
        current.GetComponent<ChangeCurrentElement>().myPlayer = myPlayer.GetComponent<PlayerInteraction>();

        //wheel
        GameObject wheel = Instantiate(elementWheel);
        wheel.transform.rotation = Camera.main.transform.rotation;
        wheel.transform.position = new Vector3(Camera.main.transform.position.x + 1.03f,
           Camera.main.transform.position.y + 0.42f,
           Camera.main.transform.position.z - 0.8f);
        wheel.transform.parent = Camera.main.transform;
        NetworkServer.Spawn(wheel);

        //life
        GameObject life = Instantiate(lifeBar);
        life.transform.rotation = Camera.main.transform.rotation;
        life.transform.position = new Vector3(Camera.main.transform.position.x + 0.83f,
Camera.main.transform.position.y - 0.37f,
Camera.main.transform.position.z + 0.37f);
        life.transform.parent = Camera.main.transform;
        NetworkServer.Spawn(life);
        life.GetComponent<UpdateLifeBar>().myPlayer = myPlayer.GetComponent<PlayerInteraction>();

    }

    [Command]
    void CmdSpawnTimer()
    {
        GameObject timer = Instantiate(Timer);
        NetworkServer.Spawn(timer);
    }

    [Command]
    void CmdSpawnArea()
    {

        
        //Get the spawn area object
        GameObject spawn = GameObject.FindGameObjectWithTag("SpawnArea");

        //only spawn the spawn area once
        if (spawn == null)
        {
            //instantiate the spawn area and then spawn it on the network
            SpawnArea = Instantiate(spawnArea);
            NetworkServer.Spawn(SpawnArea);
            //List of struct elements
            SpawnArea.GetComponent<ElementSpawn>().DetermineMinSpawnNum();
            List<ElementStruct> tempPotions = SpawnArea.GetComponent<ElementSpawn>().SpawnPotions();

            //spawn each potion
            foreach(ElementStruct p in tempPotions)
            {
                CmdSpawnPotions(p);
                elementList.Add(p);
            }
            Debug.Log("Spawn Area is Spawning");
        }
        //else
        //{
        //    //Get the element list from another server
        //    GameObject[] serverObjs = GameObject.FindGameObjectsWithTag("Server");
        //    Server server=null;
        //    foreach(GameObject s in serverObjs)
        //    {
        //        if(s.GetComponent<Server>().elementList.Count>0)
        //        {
        //            server = s.GetComponent<Server>();
        //            break;
        //        }
        //    }
        //    //set the element list
        //    if (server != null)
        //        elementList = server.elementList;
        //    //Instatiate the potions
        //    foreach(ElementStruct p in elementList)
        //    {
        //        CmdSpawnPotions(p);
        //    }
        //}
    }
 

    [Command]
    public void CmdReturnToLobby()
    {
       LobbyManager.s_Singleton.SendReturnToLobby();
    }


    [ClientRpc]
    public void RpcPlayerRespawn()
    {
        if(duplicateCheck == false)
        {
            CheckForDuplicates();
        }

        for (int i = 0; i < playerLives.Count; i++)
        {
            Debug.Log("Net ID: " + playerLives[i].netID + "  Players lives left " + playerLives[i].lives);
        }
        
        CheckWinState();
        Debug.Log("A player won");
        bool foundPlayer = false;
        int foundIndex = -1;
        for (int i = 0; i < playerLives.Count; i++)
        {
            //Debug.Log("Struct net id: " + playerLives[i].netID + " Player Net ID" + myPlayer.GetComponent<NetworkIdentity>().netId.ToString() + "Struct net id Currentlives : " + playerLives[i].lives);
            if (playerLives[i].netID == myPlayer.GetComponent<NetworkIdentity>().netId.ToString())
            {

                foundPlayer = true;
                foundIndex = i;
                i = playerLives.Count + 1;
            }
        }

        
        if (foundIndex != -1)
        {
            LivesStruct temp = playerLives[foundIndex];
            temp.lives = temp.lives - 1;
            playerLives[foundIndex] = temp;
            if (playerLives[foundIndex].lives > 0)
            {
                StartCoroutine(PlayerRespawnWait());
            }
            Debug.Log("Results of Check winstate:" + CheckWinState());
        }



        if(foundIndex != -1)
        {
            LivesStruct temp = playerLives[foundIndex];
            temp.lives = temp.lives - 1;
            playerLives[foundIndex] = temp;
            if(playerLives[foundIndex].lives > 0)
            {
                StartCoroutine(PlayerRespawnWait());
            }
        }
        
    }
    bool CheckWinState()
    {
        string potentialWinner = "";
        int someoneWon = 0;
        
        foreach (LivesStruct ls in playerLives)
        {
            Debug.Log("LS Check win state lives: " + ls.lives);
            if (ls.lives <= 0)
            {
                someoneWon++;
            }
            else
            {
                potentialWinner = ls.netID;
            }
        }
        Debug.Log("Count of player lives" + playerLives.Count + " SomeoneWon count: " + someoneWon + "Potential Winner: " + potentialWinner);

        if (someoneWon == playerLives.Count - 1)
        {
            Debug.Log(potentialWinner + " Has one the game!");
            RpcReturnToLobby();
            return true;
        }
        else
            return false;
    }
    [Command]
    public void CmdPlayerRespawn()
    {
        Debug.Log("CMDPLAYERRESPAWN");
        RpcPlayerRespawn();
    }

    IEnumerator PlayerRespawnWait()
    {
       

        System.Random rnd = new System.Random();
        int index = rnd.Next(0, spawnPoints.Count);
        yield return new WaitForSeconds(3);
        myPlayer.SetActive(true);
        myPlayer.transform.position = spawnPoints[index].position;
       // myPlayer.transform.rotation = spawnPoints[index].rotation;

    }
    
    [ClientRpc]
    public void RpcSpawnPotions(ElementStruct p)
    {
        CmdSpawnPotions(p);
    }
    [Command]
    public void CmdSpawnPotions(ElementStruct p)
    {
        ElementEnum.Elements type= (ElementEnum.Elements)p.elementType;
        GameObject temp = null;
        switch (type)
        {
            case ElementEnum.Elements.Ash:
                potionAsh.transform.position = p.position;
                potionAsh.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionAsh);
                temp.gameObject.name = "Ash";
                temp.transform.parent = null;
                break;
            case ElementEnum.Elements.Fire:
                potionFire.transform.position = p.position;
                potionFire.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionFire);
                temp.gameObject.name = "Fire";
                temp.transform.parent = null;
                break;
            case ElementEnum.Elements.Grass:
                potionGrass.transform.position = p.position;
                potionGrass.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionGrass);
                temp.gameObject.name = "Grass";
                temp.transform.parent = null;
                break;
            case ElementEnum.Elements.Water:
                potionWater.transform.position = p.position;
                potionWater.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionWater);
                temp.gameObject.name = "Water";
                break;
            case ElementEnum.Elements.Cheese:
                potionCheese.transform.position = p.position;
                potionCheese.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionCheese);
                temp.gameObject.name = "Cheese";
                temp.transform.parent = null;
                break;
        }
        if (temp != null)
            NetworkServer.Spawn(temp);
    }


}
