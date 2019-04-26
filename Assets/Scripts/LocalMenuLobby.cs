using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMenuLobby : MonoBehaviour
{
    public GameObject lobby;
    // Start is called before the first frame update
    void Start()
    {
        lobby = GameObject.FindGameObjectWithTag("Lobby");
        lobby.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateLobby()
    {
        lobby.SetActive(true);
    }
}
