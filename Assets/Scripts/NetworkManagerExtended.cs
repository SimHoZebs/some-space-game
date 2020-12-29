using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerExtended : NetworkManager
{

    public string playerName;

    //This is constantly updated at every change via Input Field in Unity.
    //It could be little more efficient to update it when the button is pressed instead.
    public void SetPlayerName(string name){
        playerName = name;
    }
    public void SetHostname(string hostName){
        networkAddress = hostName;
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        Debug.Log("Client Scnene Changed!");
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
        Debug.Log("Server scene changed!");

    }

    public override void OnStartHost()
    {
        Debug.Log("Host Started");
        base.OnStartHost();
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        GameObject playerObj = Instantiate(playerPrefab);
        playerObj.GetComponent<Player>().playerName = playerName;

        NetworkServer.AddPlayerForConnection(conn, playerObj);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        ClientScene.AddPlayer(conn);
    }

    public void OnPressHostGame(){
        StartHost();
    }

}
