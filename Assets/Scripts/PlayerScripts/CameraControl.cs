using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraControl : NetworkBehaviour
{
    [SerializeField] private Vector3 topDownViewOffset = new Vector3(0f, 10f, -5f);

    //Visualization purpose
    [Header("Debugging data")]
    [SerializeField] private bool isTransparent = false;
    [SerializeField] private float xAxis, yAxis = 0.0f;

    [Client]
    private void Update(){
        if (!isLocalPlayer){ return;}

        TopDownView();
    }

    [Client]
    private void TopDownView(){
        if (!isLocalPlayer){ return;}
        Camera.main.transform.position = transform.position + topDownViewOffset;
    }

    [Client]
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Wall")){
            other.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.CompareTag("Wall")){
            other.gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 1f);
        }
    }

}