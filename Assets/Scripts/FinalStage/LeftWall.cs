﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftWall : MonoBehaviour {

    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void StartAnimation ()
    {
        Invoke("Falling", 2.6f);
    }

    void Falling()
    {
        animator.SetTrigger(name: "Fall");
    }
}
