using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementControl : NetworkBehaviour
{

    //Speed Control
    [SerializeField] float walkAccel, runAccel, runMaxSpeed, walkMaxSpeed = 3f;

    //object instancing
    private CharacterController charController;
    private InputHandler inputHandler;

    [Client]
    void Start()
    {
        //caching gameObjs
        charController = gameObject.GetComponent<CharacterController>();
        inputHandler = gameObject.GetComponentInChildren<InputHandler>();
    }

    [Client]
    void FixedUpdate()
    {
        if (!isLocalPlayer){ return;}

        //get cam's Vector3 converted from local Z & X axis to global.
        var side = Camera.main.transform.right;
        var forward = Camera.main.transform.forward;
        //lock their Y axis to 0 because we don't wanna be moving upwards
        forward.y = side.y = 0f;

        //Basic WASD movement using vector arithmetic
        var targetVel = new Vector3(0,0,0);

        if (inputHandler.isMovingUp){
            targetVel += forward;
        }
        if (inputHandler.isMovingDown){
            targetVel -= forward;
        }
        if (inputHandler.isMovingRight){
            targetVel += side;
        }
        if (inputHandler.isMovingLeft){
            targetVel -= side;
        }

        //account for sprinting
        float currentAccel = inputHandler.isSprinting? runAccel : walkAccel;
        //Normalize vector size and multiply to target accel for constant vel
        charController.SimpleMove(Vector3.Normalize(targetVel) * currentAccel);
    }

}