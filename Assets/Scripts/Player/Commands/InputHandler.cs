using Assets.Scripts.Player.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InputHandler : NetworkBehaviour
{
    //keeping reference of all the Move Commands that the player is calling 
    Stack<Command> moves;
    public Rigidbody playerb;
    public GameObject player;
    public float Speed = 5f;

    private Timer currentTimer;

    // Start is called before the first frame update
    void Start()
    {
        moves = new Stack<Command>();
        playerb = this.gameObject.GetComponent<Rigidbody>();
        player = playerb.gameObject;
        playerb.freezeRotation = true;
        camTransform = Camera.main.transform;

        Timer[] timers = FindObjectsOfType<Timer>();

        for (int i = 0; i < timers.Length; i++)
        {
            if (timers[i].masterTimer)
            {
                currentTimer = timers[i];
            }
        }
    }

    bool undo;
    // Update is called once per frame
    void Update()
    {

        if (hasAuthority == false)
        {
            return;
        }

        if (!undo)
        {
            MouseInputForPlayerMovement();
            MouseInputForCameraRotation();
        }
        CallReplay();
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

        CameraRotationCommand cameraRotation = new CameraRotationCommand(currentRotation, target.position, dstFromTarget, target, camTransform, currentTimer.timer);
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

            PlayerRotationCommand playerRotationCommand = new PlayerRotationCommand(playerMovement, player.transform, currentTimer.timer);
            moves.Push(playerRotationCommand);
        }

        float targetSpeed = inputDir.magnitude * Speed;

        Vector3 translation = player.transform.forward * targetSpeed * Time.deltaTime;
        if (translation.magnitude > 0.001)
        {
            Move_Command moveCommand = new Move_Command(translation, player.transform, currentTimer.timer);
            moves.Push(moveCommand);
        }
    
    }

    float counter;
    //this is just being used to Test out the Replay not intended to actually be called outside of testing
    private void CallReplay()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            undo = true;
            counter = currentTimer.timer;
            print("Undo Started");
        }

        
        if (undo)
        {
            if (moves.Peek().TimeOfExcution > counter /*&& moves.Peek().TimeOfExcution > counter - 10*/) 
                // this second half will be for once this is working so its on;y the last 10 seconds of the round
            {
                //if (moves.Peek().TimeOfExcution <= counter)
                {
                    counter = moves.Peek().TimeOfExcution;
                    moves.Pop().UnExecute();
                }
            }
            else
            {
                counter -= currentTimer.masterDeltaTime;
            }
        }
    }

}
