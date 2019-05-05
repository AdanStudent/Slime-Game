using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateLifeBar : MonoBehaviour
{
    public PlayerInteraction myPlayer;
    private Animator anim;
    public int lives=3;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
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
    }
}
