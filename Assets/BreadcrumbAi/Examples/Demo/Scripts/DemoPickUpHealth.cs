using UnityEngine;
using System.Collections;

public class DemoPickUpHealth : MonoBehaviour {

	public GameObject healthCollectPrefab;
	private float bounce, pickUpSpeed;
	private bool _pickedUp;
	private GameObject player;
	
	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	void Update () {
		Bounce();
		if(player){
			if(Vector3.Distance(transform.position, player.transform.position) < 5f &&
			   Vector3.Distance(transform.position, player.transform.position) > 0.5f &&
			   !_pickedUp){
				_pickedUp = true;
			}
			if(_pickedUp){
				pickUpSpeed += 0.2f;
				transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * pickUpSpeed);
			}
			if(Vector3.Distance(transform.position, player.transform.position) <= 0.5f){
				GameObject collect = Instantiate(healthCollectPrefab, player.transform.position,Quaternion.identity) as GameObject;
				player.GetComponent<DemoPlayerControls>()._pickedUpHealth = true;
				Destroy(collect,2);
				Destroy(gameObject);
			}
		}
	}
	
	void Bounce()
	{
		const float BounceHeight = 0.02f;
		const float BounceRate = 4.0f;
		const float BounceSync = -0.75f;
		
		float t = Time.time * BounceRate + Position.x * BounceSync;
		bounce = (float)(Mathf.Sin (t)) * BounceHeight;
		transform.position = Position;
	}
	
	public Vector3 Position {
		get {
			return new Vector3 (transform.position.x,transform.position.y + bounce, transform.position.z);
		}
	}
}
