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
    public SyncListElement elementList;
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

    // Update is called once per frame
    void Update()
    {

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
        GameObject spawn = GameObject.FindGameObjectWithTag("SpawnArea");
        elementList = new SyncListElement();


        if (spawn == null)
        {
            GameObject SpawnArea = Instantiate(spawnArea);
            NetworkServer.Spawn(SpawnArea);
            List<ElementStruct> tempPotions = SpawnArea.GetComponent<ElementSpawn>().SpawnPotions();

            foreach(ElementStruct p in tempPotions)
            {
                //print(p.transform.position);
                potion.transform.position = p.position;
                potion.GetComponent<Element>().elementType = (ElementEnum.Elements)p.elementType;
                potion.GetComponent<Element>().SetMaterial();
                GameObject temp = Instantiate(potion);
                NetworkServer.Spawn(temp);
                elementList.Add(p);
            }
            Debug.Log("Spawn Area is Spawning");
        }
        else
        {
            foreach(ElementStruct p in elementList)
            {
                potion.transform.position = p.position;
                potion.GetComponent<Element>().elementType = (ElementEnum.Elements)p.elementType;
                potion.GetComponent<Element>().SetMaterial();
                GameObject temp = Instantiate(potion);
                NetworkServer.Spawn(temp);
            }
        }
    }



}
