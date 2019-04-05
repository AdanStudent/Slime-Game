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
    public int potionsInScene = 4;
    // Start is called before the first frame update
    void Start()
    {
            //if (GameObject.FindGameObjectWithTag("Potion") == null)
            //{
            //}
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

    // Update is called once per frame
    void Update()
    {
        if(potionsInScene <= 0)
        {
            foreach(GameObject go in elementReference)
            {
                go.SetActive(true);
                potionsInScene = elementsToSpawn;

            }
        }
    }

    public List<ElementStruct> SpawnPotions()
    {
        previousSpawnPoints = new List<Vector3>();
        spawnedPotions = new List<ElementStruct>();


        while (elementsSpawned < elementsToSpawn)
        {
            BoxCollider spawn = gameObject.GetComponent<BoxCollider>();

            //get the spawn point
            Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(spawn.bounds.min.x, spawn.bounds.max.x),
                spawn.transform.position.y, UnityEngine.Random.Range(spawn.bounds.min.z, spawn.bounds.max.z));


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
                //Debug.Log("Spawn");

                //reset valid boolean
                validPosition = false;
                ElementStruct temp = new ElementStruct();

                //set the material and element type of the potion
                temp.elementType=ChangeMaterial();
                //instatiate the gameobject and assign it to a variable so that it can be added as a child
                //GameObject p = Instantiate(potion, spawnPoint, transform.rotation);
                //add the potion as a child to the spawn area object
                //p.transform.parent = transform;

                temp.position = spawnPoint;

                spawnedPotions.Add(temp);
                //add the transform to the list of previously used locations
                previousSpawnPoints.Add(spawnPoint);
            }
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
