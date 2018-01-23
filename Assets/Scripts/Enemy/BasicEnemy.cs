using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public float startingHealth, power;
    private float currentHealth;

    public int maxRange;
    public int minRange = 1;

    private bool inCombat;

    public float rangedAttackNext;
    public float rangedAttackRate;

    private Transform target = null;
    public Rigidbody rangedProjectilePrefab;

    void Start()
    {
        currentHealth = startingHealth;
        //GetComponent<Animation>()["Enemy"].layer = 2;
    }

    void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }
        else
        {
            transform.LookAt(target);
            float distance = Vector3.Distance(transform.position, target.position);
            bool tooClose = distance < minRange;
            Vector3 direction = tooClose ? Vector3.back : Vector3.forward;
            transform.Translate(direction * Time.deltaTime);
            inCombat = true;
            Combat();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            target = null;
            GetComponent<Animation>().CrossFade("faceIdle");
        }
    }
    public int dmgAmount;
    public void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
        {
            Destroy(gameObject.GetComponent<Collider>());
            GetComponent<BasicEnemy>().enabled = false;
            Dead();
        }
    }

    public int forwardForce;

    void Combat()
    {
        if (Time.time > rangedAttackNext && inCombat == true)
        {
            GetComponent<Animation>().CrossFade("faceAttack");
            rangedAttackNext = Time.time + rangedAttackRate;
            Rigidbody projectile = (Rigidbody)Instantiate(rangedProjectilePrefab, transform.position + transform.forward + transform.up, transform.rotation) as Rigidbody;
            projectile.AddForce(transform.forward * forwardForce);
        }
       else
        {
            return;
        }
    }

    void Dead()
    {
        inCombat = false;
        target = null;
        GetComponent<Animation>().CrossFade("faceDeath");
        Destroy(gameObject, 3);
    }
}
