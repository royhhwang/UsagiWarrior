using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldCharacter : MonoBehaviour {

    void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.parent = gameObject.transform;
        }
        
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            collider.transform.parent = null;
        }
    }
}
