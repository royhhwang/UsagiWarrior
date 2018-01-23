using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawn : MonoBehaviour
{

    //private bool destroyProjectile;
    public float DeathTime;
    public int WeaponDamage;

    void Start()
    {
        Invoke("Despawn", DeathTime++);
    }

    void Despawn()
    {
        if (gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerHealth>().TakeDamage(WeaponDamage);
            Despawn();
        }
        else if (other.gameObject.tag == "Shield")
        {
            Despawn();
        }

    }
}
