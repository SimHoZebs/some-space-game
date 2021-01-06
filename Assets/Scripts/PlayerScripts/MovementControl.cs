using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementControl : NetworkBehaviour
{

    [Header("Speed control")]
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float sprintAccel = 3f;

    [Header("Player orientation")]
    [SerializeField] float playerViewHeight = 0f; 
    [SerializeField] float screenOffset = 0.1f;

    //object instancing
    private CharacterController charController;

    [Client]
    void Start()
    {
        if (!isLocalPlayer){ return;}
        //caching gameObjs
        charController = gameObject.GetComponent<CharacterController>();
    }

    [Client]
    void FixedUpdate()
    {
        if (!isLocalPlayer){ return;}

        FaceProperDirection();

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

    [Client]
    private void FaceProperDirection(){
        if (!isLocalPlayer){ return;}

        var camRotation = Camera.main.transform.eulerAngles;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, camRotation.y, transform.eulerAngles.z);


    }

}