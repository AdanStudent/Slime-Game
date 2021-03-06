﻿using System.Collections;
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
    public bool potionsInScene = true;
    System.Random rnd;

    // Start is called before the first frame update
    void Start()
    {
        spawnArea = GameObject.FindGameObjectWithTag("SpawnArea").GetComponent<ElementSpawn>();
        spawnArea.elementReference.Add(this.gameObject);
        if (elementType == ElementEnum.Elements.None)
        {
            RandomType();
        }

        rnd = new System.Random(Guid.NewGuid().GetHashCode());
        angles[0] = (float)rnd.NextDouble();
        angles[1] = (float)rnd.NextDouble();
        angles[2] = (float)rnd.NextDouble();

        rotationSpeed = 90;
        speed = 2f;
        source = GetComponent<AudioSource>();
    }

    float[] angles = new float[3];
    float rotationSpeed;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        //BobbingPotion();
        RotatingPotion();
    }

    private void BobbingPotion()
    {
        float newY = Mathf.Sin(Time.time * speed);
        //newY = Mathf.Clamp01(newY);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void RotatingPotion()
    {
        this.transform.RotateAround(this.transform.position, new Vector3(angles[0], angles[1], angles[2]), rotationSpeed * Time.deltaTime);
    }

    void RandomType()
    {
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

    public AudioClip Potion;
    AudioSource source;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (isServer == true)
            {
                other.GetComponent<PlayerInteraction>().RpcSetType(elementType);
            }
            else
            {
                other.GetComponent<PlayerInteraction>().CmdSetType(elementType);
            }

            source = other.GetComponent<AudioSource>();

            source.volume = 1;
            source.PlayOneShot(Potion);
            this.gameObject.SetActive(false);
        }
    }

}
