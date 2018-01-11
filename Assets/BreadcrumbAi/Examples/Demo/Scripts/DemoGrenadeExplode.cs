using UnityEngine;
using System.Collections;

public class DemoGrenadeExplode : MonoBehaviour {
	
	public GameObject bounceSound;
	public GameObject explosionPrefab;
	public GameObject monsterBloodPoolPrefab;
	public GameObject bloodPoolPrefab;
	public float radius = 5;
	private GameObject spawner;
	private AudioSource audioSource;
	private bool _HasExploded;
	private DemoScore score;
	
	void Start () {
		spawner = GameObject.Find("Spawners");
		audioSource = bounceSound.GetComponent<AudioSource>();
		Invoke("Explode",2);
		score = Camera.main.GetComponent<DemoScore>();
	}
	
	private void Explode(){
		_HasExploded = true;
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);
		Vector3 explodePos = transform.position;
		Collider[] colliders = Physics.OverlapSphere(explodePos, radius); 
		foreach (Collider hit in colliders){
			if(hit.tag == "Enemy"){
				GameObject poolPrefab;
				if(hit.GetComponent<DemoEnemyControls>().enemyType != DemoEnemyControls.EnemyType.Special){
					poolPrefab = monsterBloodPoolPrefab;
					spawner.GetComponent<DemoSpawnerControl>().enemyCount--;
					score.ScorePoint(25);
				} else {
					poolPrefab = bloodPoolPrefab;
					spawner.GetComponent<DemoSpawnerControl>().specialEnemyCount--;
					score.ScorePoint(25);
				}
				Destroy(hit.gameObject);
				GameObject bloodPool = Instantiate(poolPrefab, hit.transform.position, Quaternion.identity) as GameObject;
				Destroy(bloodPool,3);
			} else if(hit.tag == "Destructible"){
				hit.GetComponent<DemoDestroyObject>().Destruction(transform.position, 1000);
			}
		}
		Destroy(gameObject);
	}
	
	void OnCollisionEnter(){
		if(!_HasExploded && audioSource){
			audioSource.PlayOneShot(audioSource.clip);
		}
	}
}
