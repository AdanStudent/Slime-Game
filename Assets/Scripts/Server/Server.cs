using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Prototype.NetworkLobby;


public struct ElementStruct
{
    public int elementType;
    public Vector3 position;
}
public struct LivesStruct
{
    public string Name;
    public int lives;
    public LivesStruct(string name, int li)
    {
        Name = name;
        lives = li;
    }
    //GameObject 
}

public class SyncListElement : SyncListStruct<ElementStruct> { }
public class SyncListLives : SyncListStruct<LivesStruct> { }
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

    public List<Transform> spawnPoints;
    //list of elements in the scene
    [SyncVar]
    public SyncListElement elementList = new SyncListElement();
    public GameObject myPlayer;
    private GameObject SpawnArea;
    private ElementSpawn elementSpawnRef;
    //Dictionary<string, int> playerLives = new Dictionary<string, int>();
    public SyncListLives playerLives = new SyncListLives();
    // public GameObject spawnArea;
    // Start is called before the first frame update

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

  

    [Command]
    void CmdSpawnPersonalPlayer()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(0, spawnPoints.Count);
        if (connectionToClient.isReady)
        {
            myPlayer = Instantiate(playerUnit, spawnPoints[index].position, spawnPoints[index].rotation);
            NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
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
            myPlayer = Instantiate(playerUnit, spawnPoints[index].position, spawnPoints[index].rotation);
            NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
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
  
    bool CheckWinState()
    {
        string potentialWinner = "";
        int someoneWon = 0;
        //checks how many people are dead.  If someones dead then the someone won counter goes up.  If not, then their name is set as the potential winner
        foreach (LivesStruct ls in playerLives)
        {
            if(ls.lives <= 0)
            {
                someoneWon++;
            }
            else
            {
                potentialWinner = ls.Name;
            }
        }
        //checks to see if everyone is dead except one person.  If so, then that means the potential winner has one the game
        if (someoneWon == playerLives.Count - 1)
        {
            Debug.Log(potentialWinner + "Has one the game!");
            RpcReturnToLobby();
            return true;
        }
        else
            return false;
    }

    [ClientRpc]
    public void RpcReturnToLobby()
    {
        LobbyManager.s_Singleton.SendReturnToLobby();
    }

    [ClientRpc]
    public void RpcPlayerRespawn()
    {
        //checks to see if anyone won firsr
        CheckWinState();
        bool foundPlayer = false;
        int foundIndex = -1;
        for (int i = 0; i < playerLives.Count; i++)
        {
            Debug.Log("Struct name: " + playerLives[i].Name + " Player name" + myPlayer.name);
            //looks for the player who just died in the list of structs, and then takes away one of their lives
            if(playerLives[i].Name== myPlayer.name)
            {
                
                foundPlayer = true;
                foundIndex = i;
                i = playerLives.Count + 1;
            }
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
    [Command]
    public void CmdPlayerRespawn()
    {
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
