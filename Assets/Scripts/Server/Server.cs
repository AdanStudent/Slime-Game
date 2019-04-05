using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public struct ElementStruct
{
    public int elementType;
    public Vector3 position;
}

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
    public List<Transform> spawnPoints;
    //list of elements in the scene
    [SyncVar]
    public SyncListElement elementList = new SyncListElement();
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
        
    }

    [Command]
    void CmdSpawnPersonalPlayer()
    {
        System.Random rnd = new System.Random();
        int index = rnd.Next(0,spawnPoints.Count);
        GameObject myPlayer = Instantiate(playerUnit,spawnPoints[index].position,spawnPoints[index].rotation);
        NetworkServer.SpawnWithClientAuthority(myPlayer, connectionToClient);
        Debug.Log("Spawning Object");
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
            GameObject SpawnArea = Instantiate(spawnArea);
            NetworkServer.Spawn(SpawnArea);
            //List of struct elements
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
                break;
            case ElementEnum.Elements.Fire:
                potionFire.transform.position = p.position;
                potionFire.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionFire);
                break;
            case ElementEnum.Elements.Grass:
                potionGrass.transform.position = p.position;
                potionGrass.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionGrass);
                break;
            case ElementEnum.Elements.Water:
                potionWater.transform.position = p.position;
                potionWater.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionWater);
                break;
            case ElementEnum.Elements.Cheese:
                potionCheese.transform.position = p.position;
                potionCheese.GetComponent<Element>().elementType = type;
                temp = Instantiate(potionCheese);
                break;
        }
        if (temp != null)
            NetworkServer.Spawn(temp);
    }


}
