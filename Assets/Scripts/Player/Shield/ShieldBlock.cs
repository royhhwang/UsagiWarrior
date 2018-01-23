using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBlock : MonoBehaviour {
    private Animator animator;
    public bool isShieldUp;
    public PlayerHealth playerHealth;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        isShieldUp = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetButtonDown("Fire2"))
        {
            ShieldUp();
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            ShieldDown();
        }
        if (playerHealth.currentHealth <= 0)
        {
            GetComponent<ShieldBlock>().enabled = false;
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
}
