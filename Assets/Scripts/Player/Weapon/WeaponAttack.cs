using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {

    private Animator animator;
    public int WeaponDamage;
    public PlayerHealth playerHealth;

    // Use this for initialization
    void Start () 
    {
        animator = GetComponent<Animator>();
        WeaponDamage = 10;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetButtonDown("Fire1"))
        {
            BasicAttack();    
        }
        if (playerHealth.currentHealth <= 0)
        {
            GetComponent<WeaponAttack>().enabled = false;
            Destroy(this);
        }
    }

    public void BasicAttack()
    {
        animator.SetTrigger(name: "BaseAttack");
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Enemy")
        {
            col.GetComponent<IEnemy>().TakeDamage(WeaponDamage);
        }
    }
}
