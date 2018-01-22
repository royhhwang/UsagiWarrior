using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed = 15f;
    public float jumpForce = 18f;
    public float gravity = 40f;
    private Vector3 moveDir;

    public GameObject leftFoot;
    public GameObject rightFoot;

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

                CallAnimation(rightFoot, "Jump");
                CallAnimation(leftFoot, "Jump");
            }

            
        }

        if (controller.isGrounded)
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                CallAnimation(leftFoot, "Walk");
                CallAnimation(rightFoot, "Walk");
            }
            else
            {
                CallAnimation(leftFoot, "Idle");
                CallAnimation(rightFoot, "Idle");
            }
        }


        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);

    }
    
    void CallAnimation (GameObject obj, string type)
    {
        obj.GetComponent<Animator>().SetTrigger(type);
    }
}
