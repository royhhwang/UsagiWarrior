using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BreadcrumbAi{
	[System.Serializable]
	public class Ai : MonoBehaviour {
		
		#region Editor Variables
		// *****EDITOR VARIABLES***** //
		public bool _CanFollowPlayer, _CanFollowBreadcrumbs, _CanFollowAi, _CanWander, _CanPatrol,
					_CanIgnoreAi, // coming soon
					_CanWanderAnywhere,
					_CanHover,
					_CanFlee, _CanJump, _CanLongJump, _IsJumping,
					_HasAvoidance, _HasEdgeAvoidance,
					_HasVision, _HasFrontVision,
					_IsMelee, _IsRanged,
					_IsGround, _IsAir,
					_IsInvincible;
					
		public float followSpeed, wanderSpeed, patrolSpeed, rotationSpeed, avoidSpeed,
					 jumpDistance, jumpForce, longJumpForce,
					 followDistance, wanderDistance, attackDistance, visionDistance, avoidDistance, edgeDistance, otherAiDistance,
					 wanderTimeLimit, wanderTimeRate,
					 hoverHeight, hoverForce,
					 Health;
		#endregion
		
		// States are used for adding actions, animations, sounds, etc to your Ai.
		#region STATES
		public enum LIFE_STATE{
			IsAlive,
			IsDazed,	// coming soon
			IsDead,
			IsInvincible};
		public LIFE_STATE lifeState = LIFE_STATE.IsAlive;
		
		public enum VISION_STATE{
			CanSeeNothing,
			CanSeePlayer,
			CanSeeBreadcrumb,
			CanSeeFollowAi,
			CanSeeFollowAiTierTwo,
			CanSeeWaypoint};
		public VISION_STATE visionState = VISION_STATE.CanSeeNothing;
		
		public enum MOVEMENT_STATE{
			IsIdle,
			IsFollowingPlayer,
			IsFollowingBreadcrumb,
			IsFollowingAi,
			IsFollowingAiTierTwo,
			IsWandering,
			IsPatrolling};
		public MOVEMENT_STATE moveState = MOVEMENT_STATE.IsIdle;
		
		public enum ATTACK_STATE{
			CanNotAttack,
			CanAttackPlayer,
			CanAttackOther};	// coming soon
		public ATTACK_STATE attackState = ATTACK_STATE.CanNotAttack;
		#endregion
		
		// GAMEOBJECTS
		[HideInInspector]
		public Transform Player,				// Targeted Player (works with multiple players)
						 FollowingPlayer,		// Last Targeted Player
						 Breadcrumb,			// Closest Located Breadcrumb
						 FollowingAi,			// Ai To Follow
						 Waypoint,				// Current Waypoint Targeted
						 Hover,					// Hover Start Pos
						 Edge,					// Edge Avoidance Start Pos
						 LongJumpDetector,      // Jump to start Pos
						 JumpDetector;			// Jump detector
		
		// LAYERS
		[HideInInspector]
		public LayerMask  playerLayer, 		// Layer : Player
						  enemyLayer, 		// Layer : Enemy
						  breadcrumbLayer,	// Layer : Breadcrumb
						  waypointLayer,	// Layer : Waypoint
						  obstacleLayer;	// Layer : Obstacle
		
		// TAG STRINGS
		[HideInInspector]

	
		// PRIVATE VARIABLES
		private bool _IsWandering;
		private bool _IsAvoiding;				// Used for avoidance, removes velocity after avoidance
		private bool _HasWanderPos;
		private Vector3 currentWanderPos;
		private Vector3 wanderPos;				// Sets next random wander position
		private float wanderTimer, wanderNext;	// Used for timing the wander time limit
		private RaycastHit hit;
		
		
	
		void Start(){
			StartCoroutine(this.Ai_Lists());
			StartCoroutine(this.Ai_Layers());
		}
	
		void Update(){
			Ai_LifeState();
			if(IsGrounded()){
				_IsJumping = false;
			}
		}
	
		void FixedUpdate (){
			Ai_Controller(); 	// Controls Ai Movement & Attack States
			Ai_Avoidance(~(breadcrumbLayer | enemyLayer | playerLayer | waypointLayer));	// Controls Ai wall avoidance
			Ai_Hover();
		}
				
		private void Ai_Controller(){
			// Checks if following player is enabled and a player has been found	
			if(_CanFollowPlayer && this.Ai_FindPlayer()){
				_HasWanderPos = false; // TODO: this needs to be fixed
				visionState = VISION_STATE.CanSeePlayer;
				
				
				// CHANGE THIS TO FLEE (_CanFlee)
				if(_IsRanged){ // Is this a ranged ground unit?
					if(Vector3.Distance(transform.position,Player.position) > followDistance){
						moveState = MOVEMENT_STATE.IsFollowingPlayer;
						attackState = ATTACK_STATE.CanNotAttack;
						Ai_Movement(Player.position, followSpeed);
					} else if(_CanFlee && Vector3.Distance(transform.position,Player.position) <= attackDistance) {
						moveState = MOVEMENT_STATE.IsFollowingPlayer;
						attackState = ATTACK_STATE.CanNotAttack;
						Ai_Flee();
					} else {
						moveState = MOVEMENT_STATE.IsIdle;
						attackState = ATTACK_STATE.CanAttackPlayer;
						Ai_Rotation(Player.position);
					}
				} else if(_IsMelee){ // Is this a melee ground unit?
					if(Vector3.Distance(transform.position,Player.position) > followDistance){
						moveState = MOVEMENT_STATE.IsFollowingPlayer;
						attackState = ATTACK_STATE.CanNotAttack;
						Ai_Movement(Player.position, followSpeed);
					} else if(Vector3.Distance(transform.position,Player.position) <= attackDistance) {
						moveState = MOVEMENT_STATE.IsIdle;
						attackState = ATTACK_STATE.CanAttackPlayer;
						Ai_Rotation(Player.position);
					}
				}
				Debug.DrawLine(transform.position, Player.position, Color.red);
				
			// Checks if following breadcrumbs is enabled as well as if a player was spotted and a breadcrumb has been found
			} else if(_CanFollowBreadcrumbs && FollowingPlayer && this.Ai_FindBreadcrumb()){
				_HasWanderPos = false; // TODO: this needs to be fixed
				visionState = VISION_STATE.CanSeeBreadcrumb;
				moveState = MOVEMENT_STATE.IsFollowingBreadcrumb;
				attackState = ATTACK_STATE.CanNotAttack;
				Ai_Movement(Breadcrumb.position, followSpeed);
				Debug.DrawLine(transform.position, Breadcrumb.position, Color.green);
				
			// Checks if following other ai is enabled and if an ai has been found
			} else if(_CanFollowAi && this.Ai_FindAi()){
				_HasWanderPos = false; // TODO: this needs to be fixed
				visionState = VISION_STATE.CanSeeFollowAi;
				moveState = MOVEMENT_STATE.IsFollowingAi;
				attackState = ATTACK_STATE.CanNotAttack;
				if(Vector3.Distance(transform.position, FollowingAi.position) > otherAiDistance){
					Ai_Movement(FollowingAi.position,followSpeed);
				} else {
					moveState = MOVEMENT_STATE.IsIdle;
				}
				Debug.DrawLine(transform.position, FollowingAi.position, Color.magenta);
				
			// Checks if following other ai is enabled and if a tier two ai has been found	
			} else if(_CanFollowAi && this.Ai_FindAiTierTwo()){
				_HasWanderPos = false; // TODO: this needs to be fixed
				visionState = VISION_STATE.CanSeeFollowAiTierTwo;
				moveState = MOVEMENT_STATE.IsFollowingAiTierTwo;
				attackState = ATTACK_STATE.CanNotAttack;
				if(Vector3.Distance(transform.position, FollowingAi.position) > otherAiDistance){
					Ai_Movement(FollowingAi.position,followSpeed);
				} else {
					moveState = MOVEMENT_STATE.IsIdle;
				}
				Debug.DrawLine(transform.position, FollowingAi.position, Color.white);
				
			// Checks if wandering is enabled and if the timer has reached its limit
			} else if(_CanWander && wanderTimer < wanderTimeLimit) {	
				visionState = VISION_STATE.CanSeeNothing;
				attackState = ATTACK_STATE.CanNotAttack;
				Ai_Wander ();
				
			// Checks if patrolling is enabled and a waypoing has been found
			} else if(_CanPatrol && this.Ai_FindWaypoint()) {
				_HasWanderPos = false; // TODO: this needs to be fixed
				visionState = VISION_STATE.CanSeeWaypoint;
				moveState = MOVEMENT_STATE.IsPatrolling;
				attackState = ATTACK_STATE.CanNotAttack;
				Ai_Movement(Waypoint.position, patrolSpeed);
				Debug.DrawLine(transform.position, Waypoint.position, Color.yellow);
			
			// Nothing is found, reset all variables
			} else {
				Ai_Reset();
			}
		}
		
		private void Ai_Reset(){
			Player = null;
			FollowingPlayer = null;
			Breadcrumb = null;
			FollowingAi = null;
			Waypoint = null;
			wanderTimer = 0;
			moveState = MOVEMENT_STATE.IsIdle;
			attackState = ATTACK_STATE.CanNotAttack;
		}
		

		// Move the rigidbody forward based on the speed value 
		private void Ai_Movement(Vector3 position, float speed){
			if(_CanJump && CanJump()){
				if(moveState == MOVEMENT_STATE.IsFollowingPlayer || 
					moveState == MOVEMENT_STATE.IsFollowingAi ||
					moveState == MOVEMENT_STATE.IsFollowingAiTierTwo){
					Ai_Jump();
				}
			}
			if(Ai_EdgeAvoidance() && !_IsJumping){
				GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * speed);
			} else if(_CanLongJump){
				if(moveState == MOVEMENT_STATE.IsFollowingPlayer || 
				   moveState == MOVEMENT_STATE.IsFollowingAi ||
				   moveState == MOVEMENT_STATE.IsFollowingAiTierTwo){
					Ai_LongJump();
				}
			}
			Ai_Rotation(position);
		}
		
		// Rotate the Ai to look towards it's target at a set Rotation speed
		private void Ai_Rotation(Vector3 position){
			Vector3 playerPos = Vector3.zero;
			if(_IsGround){
				playerPos = new Vector3(position.x,transform.position.y,position.z); // Adjust Y position so Ai doesn't rotate up/down
			} else if(_IsAir){
				playerPos = new Vector3(position.x,position.y,position.z);
			}
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerPos - transform.position), rotationSpeed));
		}
		
		private void Ai_Flee(){
			GetComponent<Rigidbody>().MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(transform.position - Player.position), rotationSpeed));
			if(Ai_EdgeAvoidance()){
				GetComponent<Rigidbody>().MovePosition(GetComponent<Rigidbody>().position + transform.forward * Time.deltaTime * followSpeed);
			}
		}
		
		private void Ai_Jump(){
			if(IsGrounded() && !_IsJumping){
				GetComponent<Rigidbody>().AddForce((Vector3.up * jumpForce) + (transform.forward * (jumpForce/2)), ForceMode.VelocityChange);
				_IsJumping = true;
			}
		}
		
		private void Ai_LongJump(){
			if(IsGrounded() && !_IsJumping){
				if(Physics.Linecast(LongJumpDetector.position, LongJumpDetector.position + (-Vector3.up * edgeDistance))){
					GetComponent<Rigidbody>().AddForce((Vector3.up + transform.forward) * longJumpForce, ForceMode.VelocityChange );
					_IsJumping = true;
				}
				Debug.DrawLine(LongJumpDetector.position,LongJumpDetector.position + (-Vector3.up * edgeDistance));
			}
		}

		// This wander function selects a random location around the Ai and moves towards it.
		// This will be update in the future to allow specific wander radius rather than "anywhere"		
		private void Ai_Wander(){
			wanderTimer += Time.deltaTime;
			if(wanderTimer >= wanderTimeLimit){
				_IsWandering = false;
            } else {
            	_IsWandering = true;
            }
            
            if(_CanWanderAnywhere){
				currentWanderPos = transform.position;
			} else {
				if(!_HasWanderPos){
					currentWanderPos = transform.position;
					_HasWanderPos = true;
				}	
			}
            
            if(_IsWandering){
	            if(Time.time > wanderNext){
					wanderNext = Time.time + wanderTimeRate;
					float wanderX = Random.Range(currentWanderPos.x - wanderDistance, currentWanderPos.x + wanderDistance);
					float wanderZ = Random.Range(currentWanderPos.z - wanderDistance, currentWanderPos.z + wanderDistance);
					wanderPos = new Vector3(wanderX,currentWanderPos.y,wanderZ);
				}
				if(Vector3.Distance(transform.position, wanderPos) > 0.5f){
					Ai_Movement(wanderPos, wanderSpeed);
					moveState = MOVEMENT_STATE.IsWandering;
				} else {
					moveState = MOVEMENT_STATE.IsIdle;
				}
			}
		}

		// Avoidance casts a ray around the Ai so that it can bounce of walls and other obstacles
		// Velocity is set to zero so that when the AddForce is no longer being applied it will stop the Ai from sliding around
		private void Ai_Avoidance(LayerMask Layer){
			if(_HasAvoidance){
				if (Physics.Raycast(transform.position,-transform.right,out hit,avoidDistance, Layer)){
					Debug.DrawLine(transform.position, hit.point, Color.cyan);
					GetComponent<Rigidbody>().AddForce(transform.right * avoidSpeed);
					_IsAvoiding = true;
				} 
				if (Physics.Raycast(transform.position,transform.right,out hit,avoidDistance, Layer)){
					Debug.DrawLine(transform.position, hit.point, Color.cyan);
					GetComponent<Rigidbody>().AddForce(-transform.right * avoidSpeed);
					_IsAvoiding = true;
				} 
				if (Physics.Raycast(transform.position,transform.forward + -transform.right *2,out hit,avoidDistance, Layer)){
					Debug.DrawLine(transform.position, hit.point, Color.cyan);
					GetComponent<Rigidbody>().AddForce(transform.right * avoidSpeed);
					_IsAvoiding = true;	
				} 
				if (Physics.Raycast(transform.position,transform.forward + transform.right * 2,out hit,avoidDistance, Layer)){
					Debug.DrawLine(transform.position, hit.point, Color.cyan);
					GetComponent<Rigidbody>().AddForce(-transform.right * avoidSpeed);
					_IsAvoiding = true;
				} 
				if (Physics.Raycast(transform.position,-transform.forward,out hit,avoidDistance, Layer)){
					Debug.DrawLine(transform.position, hit.point, Color.cyan);
					GetComponent<Rigidbody>().AddForce(transform.forward * avoidSpeed);
					_IsAvoiding = true;
				} 
				
				// This raycast helps avoid other Ai that are directly infront
				if(Physics.Raycast(transform.position,transform.forward, out hit, transform.GetComponent<Collider>().bounds.extents.z + 0.1f)){
					if(hit.collider.tag == AiManager.enemyString){
						GetComponent<Rigidbody>().AddForce(transform.right * avoidSpeed);
						_IsAvoiding = true;
					}
				} 
				if(_IsAvoiding){
					GetComponent<Rigidbody>().velocity = Vector3.zero;
					_IsAvoiding = false;
				}
			}
		}
		
		// Self Harm Avoidance casts a ray to see if there's ground infront of the Ai, if there's no ground return false
		private bool Ai_EdgeAvoidance(){
			if(_HasEdgeAvoidance && _HasAvoidance){
				Debug.DrawLine(Edge.position, Edge.position + -Edge.up * edgeDistance);
				return Physics.Raycast(Edge.position,-Edge.up,edgeDistance);
			} else {
				return true;
			}
		}
		
		// We simply check to see if this Ai is invincible, if so then the lifestate is set to IsInvincible.
		// Otherwise check to see if the Health is equal or lower to 0 before setting to IsDead state.
		private void Ai_LifeState(){
			if(_IsInvincible){
				lifeState = LIFE_STATE.IsInvincible;
			} else {
				if(Health <= 0.0f){
					lifeState = LIFE_STATE.IsDead;
				}
			}
		}
		
		// Checks if a position is within range if vision is enabled
		public bool InRange(Vector3 position, float vision){
			if(_HasVision){
				if(Vector3.Distance(transform.position, position) < vision){
					if(_HasFrontVision){
						float visionAngle = Vector3.Dot(position - transform.position, transform.forward);
						if(visionAngle > 0){ return true;
						} else { return false; }
					} else { return true;}
				} else { return false; } 
			} else { return true; }
		}
		
		public bool CanJump(){
			return Physics.Raycast(JumpDetector.position, JumpDetector.forward, jumpDistance, obstacleLayer);
		}
		
		// This checks if the Ai is grounded, collider is required on the GameObject that has this script
		// TODO: add customizable collider in case users have different collider gameobject.
		public bool IsGrounded(){
			if(GetComponent<Collider>() != null){
				return Physics.Raycast(transform.position, -Vector3.up, GetComponent<Collider>().bounds.extents.y + 0.1f);
			} else {
				return true;
			}
		}
		
		// Still in testing, this is used to make your "ground" unity hover slightly, or hover a lot, it's customizable.
		private void Ai_Hover(){
			if(_CanHover){
				Ray hoverRay = new Ray(transform.position, -transform.up);
				RaycastHit hit;
				
				if(Physics.Raycast(hoverRay, out hit, hoverHeight)){
					float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
					Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
					GetComponent<Rigidbody>().AddForce(appliedHoverForce, ForceMode.Acceleration);
					Debug.DrawLine(Hover.position, hit.point, Color.blue);
				}
			}
		}
	}
}