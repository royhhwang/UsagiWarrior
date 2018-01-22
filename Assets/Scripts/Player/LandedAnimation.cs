using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedAnimation : MonoBehaviour {

    public GameObject player;
    PlayerMovement playerMovement;

    

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            playerMovement.playerGrounded = false;
        }
    }
    
       
    
}
