using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        //SetupLocalPlayer localPlayer = gamePlayer.GetComponent<InitialisePlayerOnNetwork>();

        //localPlayer.NameTest = lobby.playerName;
        //localPlayer.ColorTest = lobby.playerColor;
    }
}