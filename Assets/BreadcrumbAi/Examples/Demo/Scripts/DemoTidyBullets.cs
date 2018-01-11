using UnityEngine;
using System.Collections;

public class DemoTidyBullets : MonoBehaviour {

	public GameObject sparkPrefab;
	private bool _destroy, _spark;
	
	void Update(){
		if(_spark){
			GameObject spark = Instantiate(sparkPrefab,transform.position,transform.rotation) as GameObject;
			Destroy(spark,1);
		}
	
		if(_destroy){
			Destroy(gameObject);
		} else {
			Destroy(gameObject, 1);
		}
	}
	
	void OnCollisionEnter(Collision col){
		_destroy = true;
		if(col.collider.tag != "Enemy"){
			_spark = true;
		}
	}
}