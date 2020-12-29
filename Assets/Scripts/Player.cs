using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar (hook = nameof(OnNameChange))]
    public string playerName;

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public void OnNameChange(string old, string newName){
        Debug.Log($"Name changed to {newName}!");
    }


}