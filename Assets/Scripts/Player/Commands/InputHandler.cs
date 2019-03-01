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
        player.freezeRotation = true;
    }

    bool undo;
    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        if (Input.GetKeyDown(KeyCode.K))
        {
            undo = true;
        }

        if (undo)
        {
            for (int i = 0; i < moves.Count-1; i++)
            {
                moves.Pop().UnExecute();
            }
        }
    }

    Move_Command movementCommand;
    private void PlayerInput()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX > 0)
        {
            movementCommand = new Move_Command(new Vector3(1, 0), player, MoveDirection.MovePosX);
            movementCommand.Execute();
            moves.Push(movementCommand);
            
        }
        if (inputX < 0)
        {
            movementCommand = new Move_Command(new Vector3(-1, 0), player, MoveDirection.MoveNegX);
            movementCommand.Execute();
            moves.Push(movementCommand);
        }
        if (inputY > 0)
        {
            movementCommand = new Move_Command(new Vector3(0, 0, 1), player, MoveDirection.MovePosZ);
            movementCommand.Execute();
            moves.Push(movementCommand);
        }
        if (inputY < 0)
        {
            movementCommand = new Move_Command(new Vector3(0, 0, -1), player, MoveDirection.MoveNegZ);
            movementCommand.Execute();
            moves.Push(movementCommand);
        }
    }
}
