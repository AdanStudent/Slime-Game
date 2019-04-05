using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Element : NetworkBehaviour
{
    [SyncVar]
    public ElementEnum.Elements elementType = ElementEnum.Elements.None;
    public Material[] elements = new Material[5];
    public ElementSpawn spawnArea;
    // Start is called before the first frame update
    void Start()
    {
        spawnArea = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<ElementSpawn>();
        if (elementType == ElementEnum.Elements.None)
        {
            RandomType();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void RandomType()
    {
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        int typeNum = rnd.Next(0, 5);
        //randomly set element type if one was not set
        switch (typeNum)
        {
            case 0:
                elementType = ElementEnum.Elements.Ash;
                break;
            case 1:
                elementType = ElementEnum.Elements.Water;
                break;
            case 2:
                elementType = ElementEnum.Elements.Grass;
                break;
            case 3:
                elementType = ElementEnum.Elements.Fire;
                break;
            case 4:
                elementType = ElementEnum.Elements.Cheese;
                break;
        }
    }

    [Command]
    public void CmdSetMaterial()
    {

        //set the material according to the element type chosen
        switch (elementType)
        {
            case ElementEnum.Elements.Ash:
                GetComponent<Renderer>().material = elements[0];
                break;
            case ElementEnum.Elements.Fire:
                GetComponent<Renderer>().material = elements[1];
                break;
            case ElementEnum.Elements.Grass:
                GetComponent<Renderer>().material = elements[2];
                break;
            case ElementEnum.Elements.Water:
                GetComponent<Renderer>().material = elements[3];
                break;
            case ElementEnum.Elements.Cheese:
                GetComponent<Renderer>().material = elements[4];
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerInteraction>().RpcSetType(elementType);
            Destroy(gameObject);
        }
    }
}