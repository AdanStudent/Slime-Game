using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;



public class ElementSpawn : NetworkBehaviour
{
    //materials for the potions
    public Material[] elements=new Material[5];

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

    int minNumOfElementsToSpawn = 0;
    int currentElementsToSpawn;

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
        //spawn one of each element for every set of 3 unitl there are no more 3
        //if (!spawned)
        //{
        //    Debug.Log("First Spawn");
        //    RpcSpawnPotions();
        //    spawned = true;
        //}

    }

    public void DetermineMinSpawnNum()
    {
        int elements = elementsToSpawn;
        if (elements > 3)
        {
            elements--;
            for (int i = 0; i < elements; i += 3)
            {
                minNumOfElementsToSpawn++;
            }
        }
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

    }

    private void RespawnPotionsNewLocation()
    {
        List<ElementStruct> tempPotions = SpawnPotions();

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
                Collider[] colliders = Physics.OverlapSphere(spawnPoint, 1);


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
                //Make sure potions are spaced out
                foreach (Vector3 sp in previousSpawnPoints)
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

    List<Tuple<string,bool>> spawnedEqualAmount = new List<Tuple<string, bool>>()
    {
        new Tuple<string, bool>("Fire",false),
        new Tuple<string, bool>("Grass",false),
        new Tuple<string, bool>("Water",false),

    };
  
    //set the material and element type 
    public int ChangeMaterial()
    {
        int elementIndex=0;
        if (currentElementsToSpawn < minNumOfElementsToSpawn && !spawnedEqualAmount[0].Item2)
            elementIndex = 0;
        else if (currentElementsToSpawn < minNumOfElementsToSpawn && !spawnedEqualAmount[1].Item2)
            elementIndex = 1;
        else if (currentElementsToSpawn < minNumOfElementsToSpawn && !spawnedEqualAmount[2].Item2)
            elementIndex = 2;
        else if(currentElementsToSpawn < minNumOfElementsToSpawn)
            elementIndex = 3;
        else if (spawnedEqualAmount[0].Item2 && spawnedEqualAmount[1].Item2 && spawnedEqualAmount[2].Item2)
        {
            //randomly choose element type
            System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
            //range 0-3
            elementIndex = rnd.Next(0, 400);
            //for fire
            if (elementIndex <= 100)
                elementIndex = 0;
            //for grass
            else if (elementIndex > 100 && elementIndex <= 200)
                elementIndex = 1;
            //for water
            else if (elementIndex > 200 && elementIndex <= 300)
                elementIndex = 2;
            //for cheese, based on the way the code is written, this is needed because cheese only has a chance of appearing in a round
            //once or not at all to keep it fair
            else if (elementIndex > 300 && elementIndex <= 400)
                elementIndex = 3;
        }
        ElementEnum.Elements type = ElementEnum.Elements.None;

        //set the material according to the element type chosen
        switch (elementIndex)
        {
            //case 0:
            //   // potion.GetComponent<Renderer>().material = elements[0];
            //   // potion.GetComponent<Element>().elementType = ElementEnum.Elements.Ash;
            //    elementsSpawned++;
            //    type = ElementEnum.Elements.Ash;
            //    break;
            case 0:
                //potion.GetComponent<Renderer>().material = elements[1];
                //potion.GetComponent<Element>().elementType = ElementEnum.Elements.Fire;
                type = ElementEnum.Elements.Fire;
                if (currentElementsToSpawn < minNumOfElementsToSpawn)
                {
                    currentElementsToSpawn++;
                    if (currentElementsToSpawn >= minNumOfElementsToSpawn)
                    {
                        currentElementsToSpawn = 0;
                        spawnedEqualAmount[0] = new Tuple<string, bool>("Fire", true);
                    }
                }
                elementsSpawned++;
                break;
            case 1:
               // potion.GetComponent<Renderer>().material = elements[2];
                //potion.GetComponent<Element>().elementType = ElementEnum.Elements.Grass;
                type = ElementEnum.Elements.Grass;
                if (currentElementsToSpawn < minNumOfElementsToSpawn)
                {
                    currentElementsToSpawn++;
                    if (currentElementsToSpawn >= minNumOfElementsToSpawn)
                    {
                        currentElementsToSpawn = 0;
                        spawnedEqualAmount[1] = new Tuple<string, bool>("Grass", true);
                    }
                }
                elementsSpawned++;
                break;
            case 2:
                //potion.GetComponent<Renderer>().material = elements[3];
                //potion.GetComponent<Element>().elementType = ElementEnum.Elements.Water;
                type = ElementEnum.Elements.Water;
                if (currentElementsToSpawn < minNumOfElementsToSpawn)
                {
                    currentElementsToSpawn++;
                    if (currentElementsToSpawn >= minNumOfElementsToSpawn)
                    {
                        currentElementsToSpawn = 0;
                        spawnedEqualAmount[2] = new Tuple<string, bool>("Water", true);
                    }
                }
                elementsSpawned++;
                break;
                //only one cheese potion should occur if any
            case 3:
                if (!spawnedCheese)
                {
                   //potion.GetComponent<Renderer>().material = elements[4];
                   // potion.GetComponent<Element>().elementType = ElementEnum.Elements.Cheese;
                    type = ElementEnum.Elements.Cheese;
                    if (currentElementsToSpawn < 1)
                    {
                        currentElementsToSpawn++;
                        if (currentElementsToSpawn >= 1)
                        {
                            currentElementsToSpawn = minNumOfElementsToSpawn+1;
                        }
                    }
                    elementsSpawned++;
                    spawnedCheese = true;
                }
                break;
        }

        return (int)type;

    }
}
