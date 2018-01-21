using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonAdvanced : MonoBehaviour {

    public float CameraMoveSpeed, clampAngle, inputSensitivity, camDistanceXToPlayer, camDistanceYToPlayer, camDistanceZToPlayer, mouseX, mouseY, finalInputX, finalInputZ, smoothX, smoothY;
    public GameObject CameraFollowObject;
    Vector3 FollowPos;
    public GameObject CameraObject, PlayerObject;
    private float rotY = 0f;
    private float rotX = 0f;

	// Use this for initialization
	void Start () {
        CameraMoveSpeed = 120f;
        clampAngle = 40f;
        inputSensitivity = 60f;
       
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        finalInputX = inputX + mouseX;
        finalInputZ = inputZ + mouseY;

        rotY += finalInputX * inputSensitivity * Time.deltaTime;
        rotX += finalInputZ * inputSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        
    }

    

    private void LateUpdate()
    {
        CameraUpdater();
        PlayerObject.transform.eulerAngles = new Vector3(0, CameraObject.transform.eulerAngles.y, 0);

    }

    private void CameraUpdater()
    {
        Transform target = CameraFollowObject.transform;

        float step = CameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }
}
