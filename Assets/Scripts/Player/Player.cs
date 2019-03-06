using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Camera firstPlayerCam;
    public float Speed = 5;
    public InputHandler inputHandler;
    public GameObject temp;

    // Start is called before the first frame update
    void Start()
    {
        //gObject = GameObject.FindGameObjectWithTag("Main Camera");
        //inputHandler = Get<InputHandler>();
        //if (inputHandler)
        //{
        //    print("found");
        //}

        //GetComponent<Rigidbody>().freezeRotation
        
    }

    // Update is called once per frame
    void Update()
    {

        ////follow the player
        //firstPlayerCam.transform.position = temp.transform.position = this.transform.position + new Vector3(3, 0);

        ////rotate around the player without moving them
        //float angle = (Mathf.Atan2(inputHandler.h, 0) * Mathf.Rad2Deg);

        //Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //firstPlayerCam.transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Speed * Time.deltaTime);

        //temp.transform.Rotate(/*GetComponentInParent<Player>().transform.position,*/ Vector3.up, angle, Space.World);
        ////follow the player's movement forward.
    }
}
