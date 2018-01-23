using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTriggerDestroy : MonoBehaviour
{
    public FallingOnlyPlatform fallingOnlyPlatform;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            fallingOnlyPlatform.currentState = "Top";
            collider.transform.parent = gameObject.transform;
            fallingOnlyPlatform.LateChangeTarget();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            return;
        }
    }
}
