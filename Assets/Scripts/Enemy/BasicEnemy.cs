using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public float startingHealth, power;
    private float currentHealth;

    public int maxRange;
    public int minRange = 1;

    //public int WeaponDamage;
    private float rangedAttackNext = 0.0f;
    private float rangedAttackRate = 0.1f;

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
            Combat();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            target = other.transform;
            //Debug.Log("Player Hit: " + other.tag);
            //other.GetComponent<PlayerHealth>().TakeDamage(WeaponDamage);
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
    public int dmgAmount = 10;
    public void TakeDamage(int dmgAmount)
    {
        currentHealth -= dmgAmount;
        if (currentHealth <= 0)
        {
            Dead();
        }
    }

    void Combat()
    {
        if (Time.time > rangedAttackNext)
        {
            GetComponent<Animation>().CrossFade("faceAttack");
            rangedAttackNext = Time.time + rangedAttackRate;
            Rigidbody projectile = (Rigidbody)Instantiate(rangedProjectilePrefab, transform.position + transform.forward + transform.up, transform.rotation) as Rigidbody;
            projectile.AddForce(transform.forward * 1000);
        }
    }

    void Dead()
    {
        GetComponent<Animation>().CrossFade("faceDeath");
        Destroy(gameObject.GetComponent<Collider>());
        Destroy(gameObject, 3);
        target = null;
    }
}
