using UnityEngine;
using System.Collections;

public class DemoRemovePhysics : MonoBehaviour {

	void Update(){
		if(GetComponent<Rigidbody>().IsSleeping()){
			DestroyPhysics();
		} 
	}
	
	private void DestroyPhysics(){
		Destroy(transform.parent.gameObject,3);
		Destroy(GetComponent<Rigidbody>());
		Destroy(GetComponent<Collider>());
		Destroy(GetComponent<DemoRemovePhysics>());
	}
}
