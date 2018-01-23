using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;

    bool isDead;
    bool damaged;

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();
        currentHealth = startingHealth;
    }

    void Start () {
		
	}
	
	void FixedUpdate () {
        if (damaged)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        damaged = false;

        //if (playerMovement.)
    }

    public void TakeDamage (int amount)
    {
        damaged = true;
        currentHealth -= amount;

        healthSlider.value = currentHealth;
        //playerAudio.Play();
        if(currentHealth <= 0 && !isDead)
        {
            Dead();
        }
    }

    public void Dead ()
    {
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        //anim.SetTrigger("Die");

        //playerAudio.clip = deathClip;
        //playerAudio.Play();
    }
}
