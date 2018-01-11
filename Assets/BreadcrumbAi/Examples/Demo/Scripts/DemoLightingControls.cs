using UnityEngine;
using System.Collections;

public class DemoLightingControls : MonoBehaviour {

	public Transform player;
	
	void Update () {
		if(player){
			transform.LookAt(player.position);
		}
	}
}
