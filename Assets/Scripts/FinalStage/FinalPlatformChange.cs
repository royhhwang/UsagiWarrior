using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalPlatformChange : MonoBehaviour {

    private Animator animator;
    

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
   
    public void StartAnimation ()
    {
        animator.SetTrigger(name: "FinalPlatformGrow");
    }
}
