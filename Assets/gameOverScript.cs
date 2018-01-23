using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameOverScript : MonoBehaviour {

    public PlayerHealth playerHealth;
    Animator anim;

	void Start () {
        anim = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
        }
	}
}
