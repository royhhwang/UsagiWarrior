using UnityEngine;
using System.Collections;

public class DemoPlayerControls : MonoBehaviour {

	public Transform[] shotPos;
	public Transform top, bottom;
	public Rigidbody bulletPrefab, grenadePrefab;
	public GameObject footsteps, hitSound, muzFlashPrefab, bloodPrefab, bloodPoolPrefab, gameOver;
	
	[HideInInspector]public bool _isHit, _pickedUpHealth, _hasMaxHealth;
	
	private float moveSpeed = 10f,
				  fireNext = 0.0f,
				  fireRate = 0.1f,
				  fireForce = 1500f,
				  fireSpread = 10f,
				  grenadeNext = 0.0f,
				  grenadeRate = 2.0f,
				  healMaxTimer = 0.0f,
				  healTime = 0.0f,
				  healRate = 5.0f,
				  startHealth = 100,
				  maxHealth = 300,
				  currentHealth,
				  Health;
	
	private AudioSource audioFootsteps, audioHitSound;
	
	private Animator animBottom, animTop;
	private string animRun = "Run";
	private bool _stoppedMoving;
	
	void Start(){
		audioFootsteps = footsteps.GetComponent<AudioSource>();
		audioHitSound = hitSound.GetComponent<AudioSource>();
		animBottom = bottom.GetComponent<Animator>();
		animTop = top.GetComponent<Animator>();
		Health = 1;
		currentHealth = startHealth;
		healTime = healRate;
	}
	


	void Update(){
		HealthManager();
	}
	void FixedUpdate () {
		Move(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"),moveSpeed);
		WeaponsManager();
	}
	void LateUpdate(){
		Rotate();
    }
    
	private void HealthManager(){
		if(_isHit){
			healTime = 0.0f;
			Health -= 5;
			float rand = Random.value;
			if(rand <= 0.5f){
				audioHitSound.PlayOneShot(audioHitSound.clip);
			}
			_isHit = false;
		} else {
			healTime += Time.deltaTime;
			if(healTime > healRate){
				if(Health <= currentHealth){
					Health++;
				}
			}
		}
		
		if(_pickedUpHealth){
			currentHealth = maxHealth;
			Health = currentHealth;
			_hasMaxHealth = true;
			_pickedUpHealth = false;
		}
		if(_hasMaxHealth){
			healMaxTimer += Time.deltaTime;
			if(healMaxTimer > 30){
				currentHealth = startHealth;
				if(Health > currentHealth){
					Health = currentHealth;
				}
				_hasMaxHealth = false;
				healMaxTimer = 0.0f;
			}
		}
		
		if(Health <= 0f){
			Instantiate(bloodPoolPrefab, transform.position, Quaternion.identity);
			Instantiate(gameOver);
			Destroy(gameObject);
		}
	}
	
	private void WeaponsManager(){
		if(Input.GetMouseButton(0) && Time.time > fireNext){
			fireNext = Time.time + fireRate;
			StartCoroutine(Shoot());
		}
		if(Input.GetKey(KeyCode.F) && Time.time > grenadeNext){
			grenadeNext = Time.time + grenadeRate;
			ShootProjectile(grenadePrefab,shotPos[1],500,0);
		}
	}
	
	IEnumerator Shoot(){
		yield return new WaitForSeconds(0.0f);
		ShootProjectile(bulletPrefab,shotPos[0],fireForce, fireSpread);
		GameObject firstFlash = Instantiate(muzFlashPrefab, shotPos[0].position,shotPos[0].rotation) as GameObject;
		firstFlash.transform.parent = shotPos[0];
		Destroy(firstFlash, 0.5f);
		yield return new WaitForSeconds(0.1f);
		ShootProjectile(bulletPrefab,shotPos[1],fireForce, fireSpread);
		GameObject secondFlash = Instantiate(muzFlashPrefab, shotPos[1].position,shotPos[1].rotation) as GameObject;
		secondFlash.transform.parent = shotPos[1];
		Destroy(secondFlash, 0.5f);
	}
	
	public void Bleed(Quaternion rot){
		GameObject blood = Instantiate(bloodPrefab,transform.position,rot) as GameObject;
		Destroy(blood, 3);
	}
	
	private void Move(float h, float v, float speed)
	{
		if(h != 0 || v != 0){
			if(!audioFootsteps.isPlaying){
				audioFootsteps.Play();
			}
			if(h != 0 && v != 0){
				speed *= 0.75f;
			}
			
			Vector3 targetDirection = new Vector3(v+h,0,v+ -h);
			GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + targetDirection * speed * Time.deltaTime);
			GetComponent<Rigidbody>().MoveRotation(Quaternion.LookRotation(targetDirection));
			
			animTop.SetBool(animRun, true);
			animBottom.SetBool(animRun, true);
			_stoppedMoving = false;
			
		} else {
			if(!_stoppedMoving){
				GetComponent<Rigidbody>().rotation = top.rotation;
			}
			_stoppedMoving = true;
			
			audioFootsteps.Pause();
			animTop.SetBool(animRun, false);
			animBottom.SetBool(animRun, false);
		}
	}
	
	private void Rotate(){
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Plane playerPlane = new Plane(Vector3.up, transform.position);
		float hitdist = 0.0f;
		if (playerPlane.Raycast(ray, out hitdist))
		{
			Vector3 targetPoint = ray.GetPoint(hitdist);
			Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
			top.rotation = targetRotation;
        } 
    }
    
    private void ShootProjectile(Rigidbody shotPrefab, Transform shotPosition, float shotForce, float shotSpread)
	{
		Quaternion shotRotation;
		shotRotation = shotPosition.rotation;
		if(shotSpread != 0){
			float randUp = Random.Range(-shotSpread,shotSpread);
			shotRotation *= Quaternion.AngleAxis(randUp,shotPosition.up);
			float randRight = Random.Range(-shotSpread,shotSpread);
			shotRotation *= Quaternion.AngleAxis(randRight,shotPosition.right);
		}
		
		Rigidbody shot = Instantiate(shotPrefab, shotPosition.position, shotRotation) as Rigidbody;
		shot.AddForce(shot.transform.forward * shotForce);
	}
	
	void OnCollisionEnter(Collision col){
		if(col.collider.name.Contains("Spit")){
			_isHit = true;
		}
	}
	
	void OnGUI(){
		ShowHealth();
	}
	
	private void ShowHealth(){
		Texture2D lifeTexture = new Texture2D(10,10);
		int y = 0;
		while (y < lifeTexture.height){
			int x = 0;
			while(x < lifeTexture.width){
				if(Health >= currentHealth/2){
					lifeTexture.SetPixel(x,y, Color.green);
				} else {
					lifeTexture.SetPixel(x,y, Color.red);
				}
				x++;
			}
			y++;
		}
		lifeTexture.Apply();
		GUI.DrawTexture(new Rect(10,10,Screen.width/4 * Health/200,Screen.height/50),lifeTexture);
	}
}