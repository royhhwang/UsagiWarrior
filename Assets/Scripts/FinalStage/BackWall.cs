using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackWall : MonoBehaviour
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
        Invoke("Falling", 3.3f);
    }

    void Falling()
    {
        animator.SetTrigger(name: "Fall");
    }
}
