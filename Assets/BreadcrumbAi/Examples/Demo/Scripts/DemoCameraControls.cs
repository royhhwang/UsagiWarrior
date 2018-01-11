using UnityEngine;
using System.Collections;

public class DemoCameraControls : MonoBehaviour {

	public Transform player;
	private Vector3 offset;
	
	void Start () {
		transform.eulerAngles = new Vector3(60,45,0);
		offset = new Vector3(-5f,11,-5f);
	}
	
	void FixedUpdate () {
		if(player){
			transform.position = Vector3.Lerp(transform.position, player.position + offset, 0.2f);
		}
	}
}