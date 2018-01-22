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

    //void Update()
    //{
    //    if(destroyProjectile)
    //    {
    //        Despawn();
    //    } 
    //}

    void Despawn()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //target = other.transform;
            Debug.Log("Player Hit: " + other.tag);
            other.GetComponent<PlayerHealth>().TakeDamage(WeaponDamage);
            Despawn();
        }
        else if(other.gameObject.tag == "Shield")
        {
            Debug.Log("Shield Hit");
            Despawn();
        }
    }
}
