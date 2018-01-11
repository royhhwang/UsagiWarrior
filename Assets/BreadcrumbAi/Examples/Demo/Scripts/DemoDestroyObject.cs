using UnityEngine;
using System.Collections;

public class DemoDestroyObject : MonoBehaviour {

	public GameObject objectPrefab;
	private Vector3 explodePosition;	
	private float explodeForce = 200f;
	private bool _destroyed;

	void Update(){
		if(_destroyed){
			Destruction(explodePosition, explodeForce);
		}
	}

	public void Destruction(Vector3 explodePos, float force){
		GameObject pieces = Instantiate(objectPrefab, transform.position, transform.rotation) as GameObject;
		foreach(Transform child in pieces.transform){
			child.gameObject.GetComponent<Rigidbody>().AddExplosionForce(force,explodePos,5f);
		}
		Destroy (gameObject);
	}
	
	void OnCollisionEnter(Collision col){
		float rand = Random.value;
		if(col.collider.name.Contains("Bullet") && rand >= 0.5f){ // Only destroy objects 50% of the time if hit but a bullet
			explodePosition = col.collider.transform.position;
			_destroyed = true;
		}
	}	
}