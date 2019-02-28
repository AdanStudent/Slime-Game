using Assets.Scripts.Player.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //keeping reference of all the Move Commands that the player is calling 
    Stack<Move_Command> moves;

    // Start is called before the first frame update
    void Start()
    {
        moves = new Stack<Move_Command>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
