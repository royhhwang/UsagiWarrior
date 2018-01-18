using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour {
    private Animator animator;
    public int WeaponDamage;

	// Use this for initialization
	void Start () 
    {
        animator = GetComponent<Animator>();
        WeaponDamage = 10;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown("Fire1"))
        {
            BasicAttack();    
        }
	}

    public void BasicAttack()
    {
        animator.SetTrigger("BaseAttack");
    }

    private void OnTriggerEnter(Collider col)
    {

        if (col.tag == "Enemy")
        {
            Debug.Log("Hit: " + col.tag);
            col.GetComponent<IEnemy>().TakeDamage(WeaponDamage);
        }
    }
}
