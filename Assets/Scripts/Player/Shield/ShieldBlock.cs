using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour {
    private Animator animator;
    public bool isShieldUp;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        isShieldUp = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire2"))
        {
            ShieldUp();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            ShieldDown();
        }
	}

    public void ShieldUp()
    {
        animator.SetTrigger("ShieldActivate");
        isShieldUp = true;
    }

    public void ShieldDown()
    {
        isShieldUp = false;
        animator.SetTrigger("ShieldOff");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //don't take damage
        }
    }
}
