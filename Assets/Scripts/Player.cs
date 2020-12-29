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

    private void HandleMovement(){
        if (isLocalPlayer){

            var moveHorizontal = Input.GetAxis("Horizontal");
            var moveVertical = Input.GetAxis("Vertical");

            var movement= new Vector3(moveHorizontal, moveVertical, 0);

            transform.position = transform.position + movement;
        }

    }

    public void OnNameChange(string old, string newThing){
        Debug.Log($"Name changed to {newThing}!");
    }

    

    private void Update() {
        HandleMovement();
    }

}