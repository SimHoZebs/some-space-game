using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Cinemachine;

public class CameraControl : NetworkBehaviour
{
    private CinemachineFreeLook freeCam;

    [Client]
    private void Start() {
        if (!isLocalPlayer){ return;}
        freeCam = GameObject.FindGameObjectWithTag("Vcam").GetComponent<CinemachineFreeLook>();

        freeCam.Follow = transform;
        freeCam.LookAt = transform;
    }
}