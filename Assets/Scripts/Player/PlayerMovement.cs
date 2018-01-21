using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 15f;
    public float jumpForce = 18f;
    public float gravity = 40f;
    private Vector3 moveDir;

    // Use this for initialization
    void Start()
    {
       
        moveDir = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
        CharacterController controller = gameObject.GetComponent<CharacterController>();
        
        if (controller.isGrounded)
        {

            moveDir = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);

            moveDir = transform.TransformDirection(moveDir);

            moveDir *= moveSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                moveDir.y = jumpForce;
            }
        }


        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);

    }
}
