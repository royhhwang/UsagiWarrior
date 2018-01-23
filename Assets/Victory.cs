using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour {

    public bool playerWins = false;

	void Start () {
	}

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Treasure")
        {
            playerWins = true;
        }
	}
}
