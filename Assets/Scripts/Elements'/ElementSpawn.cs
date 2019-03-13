using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ElementSpawn : MonoBehaviour
{
    public Material[] elements=new Material[5];
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMaterial()
    {
        System.Random rnd = new System.Random(Guid.NewGuid().GetHashCode());
        int elementIndex = rnd.Next(0, 5);
        switch (elementIndex)
        {
            case 0:
                gameObject.GetComponent<Renderer>().material = elements[0];
                break;
            case 1:
                gameObject.GetComponent<Renderer>().material = elements[1];
                break;
            case 2:
                gameObject.GetComponent<Renderer>().material = elements[2];
                break;
            case 3:
                gameObject.GetComponent<Renderer>().material = elements[3];
                break;
            case 4:
                gameObject.GetComponent<Renderer>().material = elements[4];
                break;
        }
    }
}
