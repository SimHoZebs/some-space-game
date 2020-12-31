using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class InteractManager : NetworkBehaviour
{
    
    [SerializeField] private float interactRange = 3f;

    private void FixedUpdate() {
        if (!isLocalPlayer && !Input.GetButton("Interact")){ return;}

        var interactObj = TargetedObj();
        if (!interactObj.CompareTag("Movable")){ return;}


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
        RaycastHit hit;

        bool rayHit = Physics.Raycast(ray, out hit, interactRange);

        return rayHit? hit.transform.gameObject : null;
}

}
