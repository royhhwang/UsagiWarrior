using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameOverScript : MonoBehaviour {

    public PlayerHealth playerHealth;
    Animator anim;
    private int restartGame = 0;

    void Start () {
        anim = GetComponent<Animator>();
	}
	
	void FixedUpdate () {
		if (playerHealth.currentHealth <= 0)
        {
            anim.SetTrigger("GameOver");
            restartGame++;
            if(restartGame >= 399)
            {
                SceneManager.LoadScene("Platforms");
                restartGame = 0;
            }
        }
	}
}
