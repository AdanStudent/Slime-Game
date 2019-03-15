using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class Element : NetworkBehaviour
{
    [SyncVar]
    public ElementEnum.Elements elementType=ElementEnum.Elements.None;
    private ElementSpawn spawnArea;
    // Start is called before the first frame update
    void Start()
    {
        spawnArea = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<ElementSpawn>();
         if(elementType==ElementEnum.Elements.None)
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
        switch(typeNum)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            other.GetComponent<PlayerInteraction>().RpcSetType(elementType);
            Destroy(gameObject);
        }
    }
}
