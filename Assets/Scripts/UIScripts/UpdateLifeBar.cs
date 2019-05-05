using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLifeBar : MonoBehaviour
{
    public PlayerInteraction myPlayer;
    private GameObject currentPlayer;
    private Animator anim;
    public int lives=3;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentPlayer = GameObject.Find("LocalPlayer");
        if (currentPlayer != null)
            myPlayer = currentPlayer.GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myPlayer != null)
        {
            
            if (anim.GetInteger("Element") != (int)myPlayer.elementType)
                anim.SetInteger("Element", (int)myPlayer.elementType);
            if (anim.GetInteger("Lives") != lives)
                anim.SetInteger("Lives", lives);
        }
        else
        {
            currentPlayer = GameObject.Find("LocalPlayer");
            if (currentPlayer != null)
                myPlayer = currentPlayer.GetComponent<PlayerInteraction>();
        }
    }
}
