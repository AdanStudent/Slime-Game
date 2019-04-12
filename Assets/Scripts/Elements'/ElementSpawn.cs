using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;



public class ElementSpawn : NetworkBehaviour
{
    //materials for the potions
    public Material[] elements=new Material[5];

    //potion gameobject
    [SyncVar]
    public GameObject potion;

    //boolean that makes sure that the potions are only spawned once
    [SyncVar]
    public bool spawned=false;
    //number of elements to spawn
    public int elementsToSpawn;
    //boolean to make sure that the cheese element is only spawned once
    [SyncVar]
    private bool spawnedCheese;
    //number of elements that have been spawned
    public int elementsSpawned=0;
    //list of spawn points previously used
    private List<Vector3> previousSpawnPoints;
    //synced list of structs
    public List<ElementStruct> spawnedPotions;
    //collider for spawn area
    private BoxCollider spawnArea;
    //bool to determine if location is valid spawn point
    private bool validPosition;
    public List<GameObject> elementReference = new List<GameObject>();
    [SyncVar]
    public int potionsInScene = 4;
    private Server serverRef;
    private bool respawningPotions = false;
    // Start is called before the first frame update
    void Start()
    {

        GameObject server = GameObject.FindGameObjectWithTag("Server");
        serverRef = server.GetComponent<Server>();
        spawnedPotions = new List<ElementStruct>();
            spawnedCheese = false;
            elementsSpawned = 0;
            spawnArea = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<BoxCollider>();
            previousSpawnPoints = new List<Vector3>();
            validPosition = false;
        potionsInScene = elementsToSpawn;
            //if (!spawned)
            //{
            //    Debug.Log("First Spawn");
            //    RpcSpawnPotions();
            //    spawned = true;
            //}
       
    }
    public void Check()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        CheckIfAnyPotionsAreLeftInScene();
    }

    public void CheckIfAnyPotionsAreLeftInScene()
    {
        if (GameObject.FindGameObjectsWithTag("Potion").Length <= 0 && respawningPotions == false)
        {
            if(respawningPotions == true)
            {

            }
            else
            {
                respawningPotions = true;
                StartCoroutine(RespawnPotions());
            }
            

//             for (int i = 0; i < elementReference.Count; i++)
//             {
//                 if (elementReference[i].activeInHierarchy == true)
//                 {
//                     activeElements = true;
//                 }
//             }
//             if (activeElements == false)
//             {
//                 
//             }
        }
        
    }
    IEnumerator RespawnPotions()
    {
           respawningPotions = true;
            foreach (GameObject p in elementReference)
            {
                Destroy(p);
            }
            yield return new WaitForSecondsRealtime(1);
            List<ElementStruct> tempPotions = SpawnPotions();
            serverRef.elementList.Clear();
            foreach (ElementStruct p in tempPotions)
            {
                serverRef.CmdSpawnPotions(p);
                serverRef.elementList.Add(p);

                Debug.Log("StartingCoroutine");
            }
            respawningPotions = false;
        
        
        //serverRef.CmdPotionRespawn();
        //         foreach (GameObject go in elementReference)
        //         {
        //             go.transform.position = RespawnPotionsNewLocation();
        //             go.SetActive(true);
        //         }


    }

    private void RespawnPotionsNewLocation()
    {
        List<ElementStruct> tempPotions = SpawnPotions();

//         BoxCollider spawn = gameObject.GetComponent<BoxCollider>();
//         float randX = UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x);
//         float y = spawn.transform.position.y;
//         float randZ = UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z);
//         Vector3 spawnPoint = new Vector3(randX, y, randZ);
//         return spawnPoint;
        // 
        //             //get the spawn point
        //             Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x),
        //                 spawn.transform.position.y, UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z));
        // 
        //             //get the spawn point
        //             Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x),
        //                 spawn.transform.position.y, UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z));
        //         bool tempValidPosition = false;
        //         Vector3 finalSpawnPoint = new Vector3(0, 0, 0);
        //         while (tempValidPosition == false)
        //         {
        //             BoxCollider spawn = gameObject.GetComponent<BoxCollider>();
        // 
        //             //get the spawn point
        //             Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x),
        //                 spawn.transform.position.y, UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z));
        // 
        // 
        //             //check for overlap
        //             Collider[] colliders = Physics.OverlapSphere(spawnPoint, 4);
        // 
        //             // Go through each collider collected
        //             foreach (Collider col in colliders)
        //             {
        //                 // If this collider is tagged "Obstacle"
        //                 if (col.tag == "SpawnArea")
        //                 {
        //                     // Then this position is not a valid spawn position
        //                     tempValidPosition = true;
        //                     if (!previousSpawnPoints.Contains(spawnPoint) && tempValidPosition)
        //                     {
        //                         finalSpawnPoint = spawnPoint;
        //                     }
        //                 }
        //             }
        //             
        //         }
        //         return finalSpawnPoint;
    }

    public List<ElementStruct> SpawnPotions()
    {
        previousSpawnPoints = new List<Vector3>();
        spawnedPotions = new List<ElementStruct>();

        for (int i = 0; i < elementsToSpawn; i++)
        {
            bool foundPosition = false;
            Vector3 spawnPoint = new Vector3(0, 0, 0);
            while (!foundPosition)
            {
                BoxCollider spawn = gameObject.GetComponent<BoxCollider>();

                //get the spawn point
                spawnPoint = new Vector3(UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x),
                    spawn.transform.position.y, UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z));


                //check for overlap
                Collider[] colliders = Physics.OverlapSphere(spawnPoint, 2);

                // Go through each collider collected
                foreach (Collider col in colliders)
                {
                    // If this collider is tagged "Obstacle"
                    if (colliders.Length<=2)
                    {
                        // Then this position is not a valid spawn position
                        validPosition = true;
                    }
                    else
                    {
                        validPosition = false;
                    }
                }
                //Make sure potions are spaced out
                foreach(Vector3 sp in previousSpawnPoints)
                {
                    if(Vector3.Distance(spawnPoint,sp)<5)
                    {
                        validPosition = false;
                    }
                }
                //if it has not previously spawned at this position and is not overlapping 
                //then spawn a potion
                if (!previousSpawnPoints.Contains(spawnPoint) && validPosition)
                {
                    foundPosition = true;
                }
            }

            //Debug.Log("Spawn");

            //reset valid boolean
            validPosition = false;
            ElementStruct temp = new ElementStruct();

            //set the material and element type of the potion
            temp.elementType = ChangeMaterial();
            //instatiate the gameobject and assign it to a variable so that it can be added as a child
            //GameObject p = Instantiate(potion, spawnPoint, transform.rotation);
            //add the potion as a child to the spawn area object
            //p.transform.parent = transform;

            temp.position = spawnPoint;

            spawnedPotions.Add(temp);
            //add the transform to the list of previously used locations
            previousSpawnPoints.Add(spawnPoint);


        }

        return spawnedPotions;
    }

    //set the material and element type 
    public int ChangeMaterial()
    {
        //randomly choose element type
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        int elementIndex = rnd.Next(0, 5);
        ElementEnum.Elements type = ElementEnum.Elements.None;
        //set the material according to the element type chosen
        switch (elementIndex)
        {
            case 0:
               // potion.GetComponent<Renderer>().material = elements[0];
               // potion.GetComponent<Element>().elementType = ElementEnum.Elements.Ash;
                elementsSpawned++;
                type = ElementEnum.Elements.Ash;
                break;
            case 1:
                //potion.GetComponent<Renderer>().material = elements[1];
                //potion.GetComponent<Element>().elementType = ElementEnum.Elements.Fire;
                type = ElementEnum.Elements.Fire;
                elementsSpawned++;
                break;
            case 2:
               // potion.GetComponent<Renderer>().material = elements[2];
                //potion.GetComponent<Element>().elementType = ElementEnum.Elements.Grass;
                type = ElementEnum.Elements.Grass;
                elementsSpawned++;
                break;
            case 3:
                //potion.GetComponent<Renderer>().material = elements[3];
                //potion.GetComponent<Element>().elementType = ElementEnum.Elements.Water;
                type = ElementEnum.Elements.Water;
                elementsSpawned++;
                break;
                //only one cheese potion should occur if any
            case 4:
                if (!spawnedCheese)
                {
                   //potion.GetComponent<Renderer>().material = elements[4];
                   // potion.GetComponent<Element>().elementType = ElementEnum.Elements.Cheese;
                    type = ElementEnum.Elements.Cheese;
                    elementsSpawned++;
                    spawnedCheese = true;
                }
                break;
        }

        return (int)type;

    }
}
