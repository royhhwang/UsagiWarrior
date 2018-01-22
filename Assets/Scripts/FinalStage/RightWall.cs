using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightWall : MonoBehaviour
{

    private Animator animator;
    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void StartAnimation()
    {
        Invoke("Falling", 3);
    }

    void Falling()
    {
        animator.SetTrigger(name: "Fall");
    }
}
