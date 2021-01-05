using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementControl : NetworkBehaviour
{

    //Speed Control
    [SerializeField] float walkSpeed, sprintAccel = 3f;

    //object instancing
    private CharacterController charController;

    [Client]
    void Start()
    {
        //caching gameObjs
        charController = gameObject.GetComponent<CharacterController>();
    }

    [Client]
    void FixedUpdate()
    {
        if (!isLocalPlayer){ return;}

        //get cam's Vector3 converted from local Z & X axis to global.
        var camSide = Camera.main.transform.right;
        var camForward = Camera.main.transform.forward;
        //lock their Y axis to 0 because we don't wanna be moving upwards
        camForward.y = camSide.y = 0f;

        //Basic WASD movement using vector arithmetic
        var targetVel = (camForward * Input.GetAxis("Vertical") + camSide * Input.GetAxis("Horizontal"))* Time.deltaTime;

        //Normalize vector size and multiply to target accel for constant vel
        charController.SimpleMove(Vector3.Normalize(targetVel) * (walkSpeed + sprintAccel * Input.GetAxis("Sprint")) );
    }

}