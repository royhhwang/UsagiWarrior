using UnityEngine;
using System.Collections;

public class DemoTidySpit : MonoBehaviour {

	public GameObject explodePrefab;
	private bool _destroy;
	
	void Update(){
		if(_destroy){
			Explode();
		} else {
			Invoke("Explode", 2);
		}
	}
	
	private void Explode(){
		Destroy(gameObject);
		GameObject explode = Instantiate(explodePrefab,transform.position,transform.rotation) as GameObject;
		Destroy(explode,3);
	}
	
	void OnCollisionEnter(Collision col){
		_destroy = true;
	}
}