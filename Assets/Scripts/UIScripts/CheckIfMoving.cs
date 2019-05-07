using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfMoving : MonoBehaviour
{
    private GameObject currentPlayer;
    private Animator anim;
    private Vector3 previousPositon;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        currentPlayer = GameObject.Find("LocalPlayer");
        if (currentPlayer != null)
            previousPositon = currentPlayer.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPlayer != null)
        {
            if(transform.position!=previousPositon)
            {
                anim.SetBool("IsMoving", true);
            }
            else
            {
                anim.SetBool("IsMoving", false);
            }
            previousPositon = transform.position;
            
        }
        else
        {
            currentPlayer = GameObject.Find("LocalPlayer");
            if (currentPlayer != null)
                previousPositon = currentPlayer.transform.position;
        }
    }
}
