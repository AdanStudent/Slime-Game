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
    public GameObject potion;
    public List<Transform> spawnPoints;
    //list of elements in the scene
    [SyncVar]
    public SyncListElement elementList = new SyncListElement();
    // public GameObject spawnArea;
    // Start is called before the first frame update

    void Start()
    {
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
                //print(p.transform.position);
                potion.transform.position = p.position;
                potion.GetComponent<Element>().elementType = (ElementEnum.Elements)p.elementType;
                potion.GetComponent<Element>().CmdSetMaterial();               
                GameObject temp = Instantiate(potion);
                NetworkServer.Spawn(temp);
                elementList.Add(p);
            }
            Debug.Log("Spawn Area is Spawning");
        }
        else
        {
            //Get the element list from another server
            GameObject[] serverObjs = GameObject.FindGameObjectsWithTag("Server");
            Server server=null;
            foreach(GameObject s in serverObjs)
            {
                if(s.GetComponent<Server>().elementList.Count>0)
                {
                    server = s.GetComponent<Server>();
                    break;
                }
            }
            //set the element list
            if (server != null)
                elementList = server.elementList;
            //Instatiate the potions
            foreach(ElementStruct p in elementList)
            {
                potion.transform.position = p.position;
                potion.GetComponent<Element>().elementType = (ElementEnum.Elements)p.elementType;
                potion.GetComponent<Element>().CmdSetMaterial();
                GameObject temp = Instantiate(potion);
                NetworkServer.Spawn(temp);
            }
        }
    }



}
