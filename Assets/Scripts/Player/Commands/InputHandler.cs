using Assets.Scripts.Player.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    //keeping reference of all the Move Commands that the player is calling 
    private Stack<Command> moves;

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
        MouseInputForCameraRotation();
        //CallReplay();
    }

    //how sensitive the mouse's movement should be
    public float mouseSens = 6;
    
    //used to be the target for the camera to point at.
    //Not directly at the player
    public Transform target;

    //how far the camera should be from the player
    public float dstFromTarget = 3;

    //used to limit the min and max angles the camera can rotate
    public Vector2 pitchMinMax = new Vector2(-40, 85);

    //both are used for smoothing the rotation
    public float rotationSmoothTime = 0.12f;
    private Vector3 rotationSmoothVel;

    //keeping the currentRotation to be modified for the next frame
    private Vector3 currentRotation;

    //keeping the input's so that they are being checked against for the next frame
    private float yaw;
    private float pitch;


    public Transform camTransform;

    private void MouseInputForCameraRotation()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSens;
        pitch -= Input.GetAxis("Mouse Y") * mouseSens;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw),
            ref rotationSmoothVel, rotationSmoothTime);

        CameraRotationCommand cameraRotation = new CameraRotationCommand(currentRotation, target.position, dstFromTarget, target, camTransform);
        moves.Push(cameraRotation);
    }

    private float turnSmoothVel;
    private float turnSmoothTime = 0.1f;
    private void MouseInputForPlayerMovement()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Vector3 playerMovement = Vector3.up * Mathf.SmoothDampAngle(player.transform.eulerAngles.y, targetRotation, 
                ref turnSmoothVel, turnSmoothTime);

            player.transform.eulerAngles = playerMovement;
            PlayerRotationCommand playerRotationCommand = new PlayerRotationCommand(playerMovement, player.transform);
            moves.Push(playerRotationCommand);
        }

        float targetSpeed = inputDir.magnitude * Speed;

        Vector3 translation = player.transform.forward * targetSpeed * Time.deltaTime;
        if (translation.magnitude > 0.001)
        {
            Move_Command moveCommand = new Move_Command(translation, player.transform);
            moves.Push(moveCommand);
        }
    
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
            undo = false;
        }
    }

}
