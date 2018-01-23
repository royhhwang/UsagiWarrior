using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour, IEnemy
{
    public float startingHealth;
    private float currentHealth;
    public Transform treasure;

    public int maxRange;
    public int minRange = 1;
    public int forwardForce;
    public int dmgAmount;

    private bool inCombat;
    private bool musicPlaying = false;

    public float rangedAttackNext;
    public float rangedAttackRate;

    private Transform target = null;
    public Rigidbody rangedProjectilePrefab;

    AudioSource demonTaylor;

    void Start()
    {
        currentHealth = startingHealth;
        demonTaylor = GetComponent<AudioSource>();
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
            musicPlaying = true;
            playMusic();
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

    public void playMusic()
    {
        if (musicPlaying == true)
        {
            demonTaylor.Play();
            musicPlaying = false;
        } 
        else if (musicPlaying == false)
        {
            destroyMusic();
        }
    }

    public void destroyMusic()
    {
        if(musicPlaying == false)
        {
            Destroy(demonTaylor);
        }
    }

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
        Instantiate(treasure, transform.position, Quaternion.identity);
    }
}
