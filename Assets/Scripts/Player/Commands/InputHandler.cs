using Assets.Scripts.Player.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //keeping reference of all the Move Commands that the player is calling 
    Stack<Move_Command> moves;
    public Rigidbody player;
    public float Speed = 5f;

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
        MouseInput();
       // CallReplay();
    }

    private float turnSmoothVel;
    private float turnSmoothTime = 0.1f;
    private void MouseInput()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            Debug.Log("This will need to be refactored into Command Pattern");
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            player.transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotation, ref turnSmoothVel, turnSmoothTime);

        }
        float targetSpeed = inputDir.magnitude * Speed;
        player.transform.Translate(player.transform.forward * targetSpeed * Time.deltaTime, Space.World);
    }

    private void CallReplay()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            undo = true;
        }

        if (undo)
        {
            for (int i = 0; i < moves.Count - 1; i++)
            {
                moves.Pop().UnExecute();
            }
        }
    }

    float inputX;
    float inputY;
    Move_Command movementCommand;

    private void PlayerInput()
    {
        //inputX = Input.GetAxis("Horizontal");
        //inputY = Input.GetAxis("Vertical");

        //if (inputX > 0)
        //{
        //    movementCommand = new Move_Command(new Vector3(1, 0), player, MoveDirection.MovePosX);
        //    moves.Push(movementCommand);
            
        //}
        //if (inputX < 0)
        //{
        //    movementCommand = new Move_Command(new Vector3(-1, 0), player, MoveDirection.MoveNegX);
        //    moves.Push(movementCommand);
        //}
        //if (inputY > 0)
        //{
        //    movementCommand = new Move_Command(new Vector3(0, 0, 1), player, MoveDirection.MovePosZ);
        //    moves.Push(movementCommand);
        //}
        //if (inputY < 0)
        //{
        //    movementCommand = new Move_Command(new Vector3(0, 0, -1), player, MoveDirection.MoveNegZ);
        //    moves.Push(movementCommand);
        //}
    }
}
