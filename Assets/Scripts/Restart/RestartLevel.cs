using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartLevel : MonoBehaviour
{
    public PlayerHealth playerHealth;

    void OnTriggerEnter(Collider despawn)
    {
        if (despawn.tag == "DespawnZone")
        {
            Debug.Log("hit despawn");
            SceneManager.LoadScene("Platforms");
        }
    }
}
