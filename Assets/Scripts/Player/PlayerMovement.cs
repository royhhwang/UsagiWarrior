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
    public bool playerGrounded;
    public bool playerFalling;

    public GameObject body;

    Animator animator;
    

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        moveDir = Vector3.zero;
    }

   
    // Update is called once per frame
    void Update()
    {
        CharacterController controller = body.GetComponentInParent<CharacterController>();
        
        
        if (controller.isGrounded)
        {
            moveDir = new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
            moveDir = transform.TransformDirection(moveDir);
            moveDir *= moveSpeed;

            if (Input.GetButtonDown("Jump"))
            {
                moveDir.y = jumpForce;
                animator.SetTrigger("Jump");
            }
            
            if (moveDir == new Vector3 (0, 0, 0))
            {
                animator.SetTrigger("Idle");
            }
            else if (moveDir.y == 0)
            { 
                animator.SetTrigger("Walk");
            }
        }
        moveDir.y -= gravity * Time.deltaTime;

        controller.Move(moveDir * Time.deltaTime);
        
        
    }
    
}


