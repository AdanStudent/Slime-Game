using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCurrentElement : MonoBehaviour
{
    public PlayerInteraction myPlayer;
    private GameObject currentPlayer;
    private Animator currentUI;

    // Start is called before the first frame update
    void Start()
    {
        currentUI = GetComponent<Animator>();
        currentPlayer = GameObject.Find("LocalPlayer");
        if(currentPlayer!=null)
            myPlayer = currentPlayer.GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if(myPlayer!=null)
        {
            if (currentUI.GetInteger("Element") != (int)myPlayer.elementType)
                currentUI.SetInteger("Element", (int)myPlayer.elementType);
        }
        else
        {
            currentPlayer = GameObject.Find("LocalPlayer");
            if (currentPlayer != null)
                myPlayer = currentPlayer.GetComponent<PlayerInteraction>();
        }
    }
}
