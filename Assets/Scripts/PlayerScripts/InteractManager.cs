using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractManager : NetworkBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private bool playerInteracting = false;
    [SerializeField] private GameObject interactObj = null;
    [SerializeField] private int interactObjPosOffset = 3;
    private RaycastHit hitObj;

    [Client]
    private void Update() {
        //Check if update is called by local player and if they pressed interact button
        if (!isLocalPlayer ){ return;}

        //If player isn't interacting and obj is not null, drag the obj around
        //Player either presses interact or/and interactObj is null, below is false.
        if(!Input.GetButtonDown("Interact") && interactObj != null){
            interactObj.transform.position = Camera.main.transform.position + Camera.main.transform.forward * interactObjPosOffset;
            return;
        }

        //if player presses interact run below. If it didn't, if statement is false
        if (interactObj != null){
            //Drop the obj
            interactObj = null;
            playerInteracting = false;
        }
        //if player pressed interact and obj is null, then get that obj.
        //If player didn't and obj is null, below is false.
        else if (Input.GetButtonDown("Interact")){
            interactObj = TargetedObj();
        }

    }

    [Client]
    public GameObject TargetedObj(){
        if (!isLocalPlayer){ return null;}
        //TargetObj can be null, and should have no response if it is.

        //ray stores information about how a ray should look
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //Physics.Raycast casts the ray using that info
        //and returns a bool whether something collided within interactRange
        //assigns value Vector3 direction to hit

        bool rayHit = Physics.Raycast(ray, out hitObj, interactRange);

        return rayHit? hitObj.transform.gameObject : null;
}

}
