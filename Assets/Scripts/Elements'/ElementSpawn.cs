using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ElementSpawn : NetworkBehaviour
{
    public Material[] elements=new Material[5];
    public GameObject potion;
    public int elementsToSpawn;
    private bool spawnedCheese;
    public int elementsSpawned=0;
    private List<Vector3> previousSpawnPoints;
    private BoxCollider spawnArea;
    private bool validPosition;

    // Start is called before the first frame update
    void Start()
    {
        spawnedCheese = false;
        elementsSpawned = 0;
        spawnArea = GetComponent<BoxCollider>();
        previousSpawnPoints = new List<Vector3>();
        validPosition = false;
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
            Vector3 spawnPoint = new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                spawnArea.transform.position.y, UnityEngine.Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z));

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
            if (!previousSpawnPoints.Contains(spawnPoint)&&validPosition)
            {
                Debug.Log("Spawn");
                validPosition = false;
                ChangeMaterial();
                GameObject p=Instantiate(potion, spawnPoint, transform.rotation)as GameObject;
                p.transform.parent = transform;
                NetworkServer.Spawn(p);
                previousSpawnPoints.Add(spawnPoint);
            }
        }
    }

    public void ChangeMaterial()
    {
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        int elementIndex = rnd.Next(0, 5);
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
