using Assets.Scripts.Player.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //keeping reference of all the Move Commands that the player is calling 
    Stack<Command> moves;

    public Rigidbody player;
    public float Speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        moves = new Stack<Command>();
        player.freezeRotation = true;
    }

    bool undo;
    // Update is called once per frame
    void Update()
    {

        MouseInputForPlayerMovement();
       // CallReplay();
    }

    private float turnSmoothVel;
    private float turnSmoothTime = 0.1f;
    private void MouseInputForPlayerMovement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            Debug.Log("This will need to be refactored into Command Pattern");
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 playerMovement = Vector3.up * Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotation, 
                ref turnSmoothVel, turnSmoothTime);

            player.transform.eulerAngles = playerMovement;
            PlayerRotationCommand playerRotationCommand = new PlayerRotationCommand(playerMovement, player.transform);
            moves.Push(playerRotationCommand);
        }

        float targetSpeed = inputDir.magnitude * Speed;

        Vector3 translation = player.transform.forward * targetSpeed * Time.deltaTime;

        Move_Command moveCommand = new Move_Command(translation, player.transform);
        moves.Push(moveCommand);
    
    }

    //this is just being used to Test out the Replay not intended to actually be called outside of testing
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

}
