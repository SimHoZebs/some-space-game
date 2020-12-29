using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerExtended : NetworkManager
{
    public string typedName;

    public struct PlayerNameMessage: NetworkMessage{
        public string playerName;
    };

    //This is constantly updated at every change via Input Field in Unity.
    //It could be little more efficient to update it when the button is pressed instead.
    public void SetPlayerName(string name){
        typedName = name;
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
        NetworkServer.RegisterHandler<PlayerNameMessage>(AssignPlayerNameMessage);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("OnServerAddPlayer called");
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect called");
        ClientScene.AddPlayer(conn);

        conn.Send(new PlayerNameMessage {playerName = typedName});
    }

    private void AssignPlayerNameMessage(NetworkConnection connection, PlayerNameMessage playerNameMessage){
        GameObject playerObj = Instantiate(playerPrefab);

        playerObj.GetComponent<Player>().playerName = playerNameMessage.playerName;

        NetworkServer.AddPlayerForConnection(connection, playerObj);
    }

    public void OnPressHostGame(){
        StartHost();
    }

}
