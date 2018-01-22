using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTriggerPlatforms : MonoBehaviour
{
    public FallingPlatforms fallingPlatforms;
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            fallingPlatforms.currentState = "Top";
            collider.transform.parent = gameObject.transform;
            fallingPlatforms.LateChangeTarget();
        }

    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            fallingPlatforms.currentState = "Bottom";
            collider.transform.parent = null;
            fallingPlatforms.ChangeTarget();
        }
    }
}
