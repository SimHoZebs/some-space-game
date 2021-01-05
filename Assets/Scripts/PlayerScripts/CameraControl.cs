using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class CameraControl : NetworkBehaviour
{
    private CinemachineFreeLook freeCam;

    //Visualization purpose
    [Header("Debugging data")]
    [SerializeField] private float xAxis, yAxis = 0.0f;

    [Client]
    private void Start() {
        if (!isLocalPlayer){ return;}
        freeCam = GameObject.FindGameObjectWithTag("Vcam").GetComponent<CinemachineFreeLook>();

        freeCam.Follow = transform;
        freeCam.LookAt = transform;
    }
}