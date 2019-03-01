using Assets.Scripts.Player.Commands;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //keeping reference of all the Move Commands that the player is calling 
    Stack<Move_Command> moves;
    public Rigidbody player;

    // Start is called before the first frame update
    void Start()
    {
        moves = new Stack<Move_Command>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    Move_Command movementCommand;
    private void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            movementCommand = new Move_Command(new Vector3(1, 0), player, MoveDirection.MovePosX);
            movementCommand.Execute();
            moves.Push(movementCommand);
            
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            movementCommand = new Move_Command(new Vector3(-1, 0), player, MoveDirection.MoveNegX);
            movementCommand.Execute();
            moves.Push(movementCommand);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            movementCommand = new Move_Command(new Vector3(0, 0, 1), player, MoveDirection.MovePosZ);
            movementCommand.Execute();
            moves.Push(movementCommand);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            movementCommand = new Move_Command(new Vector3(0, 0, -1), player, MoveDirection.MoveNegZ);
            movementCommand.Execute();
            moves.Push(movementCommand);
        }
    }
}
