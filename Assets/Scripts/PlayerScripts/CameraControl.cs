using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraControl : NetworkBehaviour
{
    [Header("Top down view settings")]
    [SerializeField] private Vector3 topDownViewOffset = new Vector3(0f, 10f, -5f);

    [Header("Object Translucency")]
    [SerializeField] private Vector3 raycastOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private float colorAlphaLevel = 0.2f;
    [SerializeField] private Material prevHitObjMat;
    [SerializeField] private Color prevHitObjColor, newObjColor;

    //Visualization purpose
    [Header("Debugging data")]
    [SerializeField] private float xAxis, yAxis = 0.0f;

    [Client]
    private void Update(){
        if (!isLocalPlayer){ return;}

        ObjectTransparency();
        TopDownView();

    }

    [Client]
    private void TopDownView(){
        if (!isLocalPlayer){ return;}
        Camera.main.transform.position = transform.position + topDownViewOffset;
    }

    [Client]
    private void ObjectTransparency(){
        if (!isLocalPlayer){ return;}
        RaycastHit hit;
        var mainCamPos = Camera.main.transform.position;
        Vector3 rayDirection = -mainCamPos + transform.position + raycastOffset;

        //Drawing the raycast in scene view
        Debug.DrawRay(mainCamPos, rayDirection, Color.green);

        bool rayHit = Physics.Raycast(mainCamPos, rayDirection, out hit);

        if (!hit.transform.CompareTag("Player")){
            prevHitObjMat = hit.transform.gameObject.GetComponent<MeshRenderer>().material;

            if (prevHitObjMat.color.a == colorAlphaLevel){ return;}

            newObjColor = prevHitObjColor = prevHitObjMat.color;
            newObjColor.a = colorAlphaLevel;
            prevHitObjMat.color = newObjColor;
        }
        else if(prevHitObjMat != null){
            Debug.Log("reverting color");
            prevHitObjMat.color = prevHitObjColor;
            prevHitObjMat = null;
        }
    }

}