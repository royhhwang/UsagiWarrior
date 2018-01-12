using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraMovement : MonoBehaviour
{

    public Transform playerCam, character, centerPoint;


    private float mouseX, mouseY;
    public float mouseSensitivity = 10f;
    public float mouseYPosition = 1f;

    private float zoom;
    public float zoomSpeed = 2;
    public float zoomMin = -2f;
    public float zoomMax = -10f;
    public float rotationSpeed = 5f;

    private int target = 10;

    private PlayerMovement playerMove;

    private void Awake()
    {
        Cursor.visible = false;
    }
    private void Start()
    {
        zoom = -3;
    }

    private void Update()
    {

        zoom += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (zoom > zoomMin)
        {
            zoom = zoomMin;
        }

        if (zoom < zoomMax)
        {
            zoom = zoomMax;
        }

        mouseY = Mathf.Clamp(mouseY, -40f, 40f);

        playerCam.transform.localPosition = new Vector3(0, 0, zoom);
        playerCam.LookAt(centerPoint);
        centerPoint.localRotation = Quaternion.Euler(mouseY, mouseX, 0);


        mouseX += Input.GetAxis("Mouse X");
        mouseY -= Input.GetAxis("Mouse Y");

        centerPoint.position = new Vector3(character.position.x, character.position.y, character.position.z);


        if (Mathf.Abs(Input.GetAxis("Mouse X")) > 0.0001f)
        {

            Quaternion turnAngle = Quaternion.Euler(0, centerPoint.eulerAngles.y, 0);

            character.rotation = Quaternion.Slerp(character.rotation, turnAngle, Time.deltaTime * 5f);

            target = 50;
        

        }
        else if (character.eulerAngles.y != centerPoint.eulerAngles.y && target >0)
        {
            Quaternion turnAngle = Quaternion.Euler(0, centerPoint.eulerAngles.y, 0);

            character.rotation = Quaternion.Slerp(character.rotation, turnAngle, Time.deltaTime* 5f);

            target -= 1;
        }




            //else
            //{
            //    Debug.Log("Doing other");
            //    character.rotation = Quaternion.Slerp(character.rotation, character.rotation, Time.deltaTime * rotationSpeed);
            //}






        }
}
