using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enabler : MonoBehaviour
{
    public CustomNetworkManager CNM;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(CNM.enabled == false)
        {
            CNM.enabled = true;
        }
    }
}
