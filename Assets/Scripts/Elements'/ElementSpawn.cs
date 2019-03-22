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
    public List<GameObject> spawnedPotions;
    //collider for spawn area
    private BoxCollider spawnArea;
    //bool to determine if location is valid spawn point
    private bool validPosition;

    // Start is called before the first frame update
    void Start()
    {
            //if (GameObject.FindGameObjectWithTag("Potion") == null)
            //{
            //}
            spawnedPotions = new List<GameObject>();
            spawnedCheese = false;
            elementsSpawned = 0;
            spawnArea = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<BoxCollider>();
            previousSpawnPoints = new List<Vector3>();
            validPosition = false;
            //if (!spawned)
            //{
            //    Debug.Log("First Spawn");
            //    RpcSpawnPotions();
            //    spawned = true;
            //}
            CmdSpawnPotions();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    [Command]
    public void CmdSpawnPotions()
    {

        while (elementsSpawned < elementsToSpawn)
        {
            //get the spawn point
            Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                spawnArea.transform.position.y, UnityEngine.Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));

            //check for overlap
            Collider[] colliders = Physics.OverlapSphere(spawnPoint, 4);

            // Go through each collider collected
            foreach (Collider col in colliders)
            {
                // If this collider is tagged "Obstacle"
                if (col.tag == "SpawnArea")
                {
                    // Then this position is not a valid spawn position
                    validPosition = true;
                }
            }
            //if it has not previously spawned at this position and is not overlapping 
            //then spawn a potion
            if (!previousSpawnPoints.Contains(spawnPoint) && validPosition)
            {
                Debug.Log("Spawn");
                //reset valid boolean
                validPosition = false;

                //set the material and element type of the potion
                ChangeMaterial();
                //instatiate the gameobject and assign it to a variable so that it can be added as a child
                GameObject p = Instantiate(potion, spawnPoint, transform.rotation);
                //add the potion as a child to the spawn area object
                //p.transform.parent = transform;
                spawnedPotions.Add(p);
                //add the transform to the list of previously used locations
                previousSpawnPoints.Add(spawnPoint);
            }
        }
    }

    //set the material and element type 
    public void ChangeMaterial()
    {
        //randomly choose element type
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        int elementIndex = rnd.Next(0, 5);
        //set the material according to the element type chosen
        switch (elementIndex)
        {
            case 0:
                potion.GetComponent<Renderer>().material = elements[0];
                potion.GetComponent<Element>().elementType = ElementEnum.Elements.Ash;
                elementsSpawned++;
                break;
            case 1:
                potion.GetComponent<Renderer>().material = elements[1];
                potion.GetComponent<Element>().elementType = ElementEnum.Elements.Fire;
                elementsSpawned++;
                break;
            case 2:
                potion.GetComponent<Renderer>().material = elements[2];
                potion.GetComponent<Element>().elementType = ElementEnum.Elements.Grass;
                elementsSpawned++;
                break;
            case 3:
                potion.GetComponent<Renderer>().material = elements[3];
                potion.GetComponent<Element>().elementType = ElementEnum.Elements.Water;
                elementsSpawned++;
                break;
                //only one cheese potion should occur if any
            case 4:
                if (!spawnedCheese)
                {
                    potion.GetComponent<Renderer>().material = elements[4];
                    potion.GetComponent<Element>().elementType = ElementEnum.Elements.Cheese;
                    elementsSpawned++;
                    spawnedCheese = true;
                }
                break;
        }



    }
}
