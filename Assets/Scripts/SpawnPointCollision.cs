using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCollision : MonoBehaviour
{

    public bool anotherPlayerIsHere = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerIsColliding()
    {
        anotherPlayerIsHere = true;
    }

    public void PlayerStoppedColliding()
    {
        anotherPlayerIsHere = false;
    }

   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerIsColliding();
            Debug.Log("Player is currently in Spawnpoint");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerStoppedColliding();
            Debug.Log("Player is currently out of spawn point");
        }
    }
}
