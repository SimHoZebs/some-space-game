using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MovementControl : NetworkBehaviour
{
    //Controls movement and rotation of player model.

    [Header("Speed control")]
    [SerializeField] public float walkSpeed = 4f;
    [SerializeField] public float accel = 2f;
    [SerializeField] public float crouchSlow = 2f;

    [Header("Player orientation")]
    [Tooltip("Player faces at a point coming from the screen. This slightly ofsets the angle the player looks at to reduce curving while moving.")]
    [SerializeField] private float lookAtAngleOffset = 10f;

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
    void Update()
    {
        if (!isLocalPlayer){ return;}
        CorrectModelRotation();
        Move();
    }

    [Client]
    private void CorrectModelRotation(){

        Vector3 faceRotation = transform.eulerAngles;
        Vector3 camRotation = Camera.main.transform.eulerAngles;

        faceRotation.y = Input.GetButton("Vertical") || Input.GetButton("Horizontal")
            ? camRotation.y - lookAtAngleOffset 
            : faceRotation.y;

        transform.eulerAngles = faceRotation;

    }

    [Client]
    private void Move(){

        //left-right movement is based on camera's relative x axis.
        var relativeSide = Camera.main.transform.right * Input.GetAxis("Horizontal") * Time.deltaTime;

        //forward-back movement is based on player mode's relative z axis.
        var relativeForward = transform.forward * Input.GetAxis("Vertical") * Time.deltaTime;

        //lock their Y axis to 0 because we don't wanna be moving upwards
        relativeForward.y = relativeSide.y = 0f;

        var normalizedMoveVector = Vector3.Normalize(relativeSide + relativeForward);
        var finalMoveVector = normalizedMoveVector * (walkSpeed + accel * Input.GetAxis("Accel"));

        charController.SimpleMove(finalMoveVector);
    }
}