using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class despawn : MonoBehaviour {

    public float DeathTime; //This implies a delay of 2 seconds.

    void Start()
    {
        Invoke("Despawn", DeathTime++);
    }

    void Despawn()
    {
        if (DeathTime >= 5)
        {
            Destroy(gameObject);
        }
    }
}
