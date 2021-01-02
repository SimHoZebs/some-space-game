using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class NetworkManagerExtended : NetworkManager
{

    [SerializeField] private bool debugMode = false;
    [SerializeField] private Transform spawnPoint;
    private string typedName;

    public override void Start()
    {
        base.Start();

        if (debugMode){
            typedName = "DebugMode";
            StartHost();
        }
    }

    //NetworkMessage to send playerName data collected from here to the created local player
    public struct PlayerNameMessage: NetworkMessage{
        public string playerName;
    };

    //This is constantly updated at every change via Input Field in Unity.
    //It could be little more efficient to update it when the button is pressed instead.
    public void SetPlayerName(string _){
        typedName = _;
    }
    public void SetNetworkAddress(string _){
        networkAddress = _;
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        Debug.Log("Client Scnene Changed!");
    }

    public override void OnStartHost()
    {
        Debug.Log("Host Started");
        base.OnStartHost();
        NetworkServer.RegisterHandler<PlayerNameMessage>(AssignPlayerNameMessage);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        Debug.Log("OnClientConnect called");
        ClientScene.AddPlayer(conn);

        conn.Send(new PlayerNameMessage {playerName = typedName});
    }

    private void AssignPlayerNameMessage(NetworkConnection connection, PlayerNameMessage playerNameMessage){
        Debug.Log("AssignPlayerNameMessage");
        GameObject playerObj = Instantiate(playerPrefab);

        playerObj.GetComponent<Player>().playerName = playerNameMessage.playerName;

        NetworkServer.AddPlayerForConnection(connection, playerObj);
    }

    public override void OnServerSceneChanged(string sceneName)
    {
        Debug.Log("Server scene changed!");
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("OnServerAddPlayer called!");
    }

}
