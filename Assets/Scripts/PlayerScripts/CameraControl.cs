using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CameraControl : NetworkBehaviour
{

    [Header("Head-Follow Mode")]
    [SerializeField] private int headFollowModeId = 0;
    [SerializeField] private Vector3 headFollowModeCamOffset = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private float camSensitivity = 2.0f;
    //[SerializeField] private Transform playerTransform;

    [Header("Interaction Mode")]
    [SerializeField] private int taskModeId = 1;
    [SerializeField] private GameObject focusObj;
    [SerializeField] private Vector3 taskModeCamOffset = new Vector3(0f, 0f, -2f);
    //interact with public variables

    //Visualization purpose
    [Header("Debugging data")]
    [SerializeField] private float xAxis, yAxis = 0.0f;
    [SerializeField] private int currCamMode = 0;

    [Client]
    private void Update(){
        if (!isLocalPlayer){ return;}

        if (currCamMode == headFollowModeId){
            HeadFollowMode();
        }
        else if(currCamMode == taskModeId){
            TaskMode(focusObj);
        }
    }

    [Client]
    public void ChangeCamMode(GameObject taskObj){

        if (taskObj == null){
            Debug.Log("Normal Mode");
            currCamMode = headFollowModeId;
            focusObj = null;
        }
        else{
            Debug.Log("Interaction Mode");
            currCamMode = taskModeId;
            focusObj = taskObj;
        }
    }

    [Client]
    private void HeadFollowMode(){
        if (!isLocalPlayer){ return;}
        //Have same position as the attachTarget with an offset
        Camera.main.transform.position = transform.position + headFollowModeCamOffset;

        //get mouse coords
        xAxis += camSensitivity * Input.GetAxis("Mouse X");
        yAxis -= camSensitivity * Input.GetAxis("Mouse Y");

        //convert mouse coords to eulerAngles 
        Camera.main.transform.eulerAngles = new Vector3(yAxis, xAxis, 0.0f);

        //same I too don't know wtf Euler Angles are but it works
        //Thanks StackOverflow
    }

    private void TaskMode(GameObject taskObj){
        Camera.main.transform.position = taskObj.transform.position + taskModeCamOffset;
    }
}
