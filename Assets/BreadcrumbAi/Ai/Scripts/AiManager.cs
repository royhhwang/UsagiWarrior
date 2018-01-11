using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BreadcrumbAi{
	public static class AiManager {
	
		static private RaycastHit hit;
		static private GameObject[] players, breadcrumbs, waypoints;
		static public string playerString = "Player",
							 enemyString = "Enemy",
							 breadcrumbString = "Breadcrumb",
							 waypointString = "Waypoint",
							 obstacleString = "Obstacle";
        
        /*
        	This coroutine runs every 0.2 seconds to create an array of objects for player, breadcrumbs and waypoints
        	If you do not have your Tags or Layers setup, an exception will pop up explaining what you need to do!
        */
        
        public static IEnumerator Ai_Lists(this Ai ai){
			while(ai.enabled){
				try{
				players = GameObject.FindGameObjectsWithTag(playerString);
				breadcrumbs = GameObject.FindGameObjectsWithTag(breadcrumbString);
				waypoints = GameObject.FindGameObjectsWithTag(waypointString);
				} catch(UnityException ex){
					Debug.Log(ex.Message + "\n" +
											" Please go to Edit > Project Settings > Tags and Layers\n" +
											"Maximize the Tag section and add the proper Tags.\n" +
					          				"Maximize the Layers section and add the proper Layers.\n" +
											"You can find the right Tags and Layers in the ReadMe file\n" +
											"Go to BreadcrumbAi > Ai > Documentation > ReadMe\n\n");
				}
				yield return new WaitForSeconds(0.2f);
			}
		}
		
		// Set up the layermasks to specific layers
		public static IEnumerator Ai_Layers(this Ai ai){
			ai.playerLayer = (1 << LayerMask.NameToLayer(playerString));
			ai.enemyLayer = (1 << LayerMask.NameToLayer(enemyString));
			ai.breadcrumbLayer = (1 << LayerMask.NameToLayer(breadcrumbString));
			ai.waypointLayer = (1 << LayerMask.NameToLayer(waypointString));
			ai.obstacleLayer = (1 << LayerMask.NameToLayer(obstacleString));
			
			yield return null;
		}
		
		/*
			Here we're looking for the closest player to the Ai.
			If we're already following a player, just make sure it's in our line of sight and return the current player
			If something is in the way, look for another player
			Cast a line to all players and if the line hits the player, see which player hit is closest to you
			return the players position
		*/
		
		public static Transform Ai_FindPlayer(this Ai ai) {
			if(ai.Player){
				if(ai.InRange(ai.Player.position, ai.visionDistance)){
					if(Physics.Linecast(ai.transform.position,ai.Player.position, out hit, ~(ai.breadcrumbLayer | ai.waypointLayer))){
						if(hit.collider.tag == playerString){
							if(players != null){
								foreach(GameObject p in players){
									if(p == hit.collider.gameObject){
										ai.Player = p.transform;
										return ai.Player;
									}
								}
							}
						} else {
							ai.Player = null;
						}
					}
				} else {
					ai.Player = null;
				}
			}	
			float distance = Mathf.Infinity;
			if(players != null){
				foreach(GameObject p in players){
					if(p && ai.InRange(p.transform.position, ai.visionDistance)){
						if(Physics.Linecast(ai.transform.position,p.transform.position, out hit, ~(ai.breadcrumbLayer | ai.waypointLayer))){
							if(hit.collider.tag == playerString){
								Vector3 diff = p.transform.position - ai.transform.position;
								float curDistance = diff.sqrMagnitude;
								if (curDistance < distance){
									ai.Player = p.transform;
									ai.FollowingPlayer = p.transform;
									distance = curDistance;
								}
							}
						}
					}
				}
			}
			return ai.Player;
		}
		
		/*
			Here we're looking for Breadcrumbs that the player leaves behind.
			First we see if we're already looking at a breadcrumb, if we are and we haven't reached it yet, return the breadcrumb
			If we don't have a breadcrumb or something is blocking the current one (or it doesn't exist anymore)
			then we look for a new one. The Ai looks for a breadcrumb that's visible and closest to the player.
		*/
		public static Transform Ai_FindBreadcrumb(this Ai ai){
			if(ai.Breadcrumb){
				if(ai.InRange(ai.Breadcrumb.position, ai.visionDistance)){
					if(Physics.Linecast(ai.transform.position, ai.Breadcrumb.position, out hit, ~(ai.waypointLayer))){
						if(hit.collider.tag == breadcrumbString){
							if(Vector3.Distance(ai.transform.position, ai.Breadcrumb.position) > 0.5f){
								return ai.Breadcrumb;
							} else {
								ai.Breadcrumb = null;
							}
						} else {
							ai.Breadcrumb = null;
						}
					}
				} else {
					ai.Breadcrumb = null;
				}
			}
			float distance = Mathf.Infinity;
			if(breadcrumbs != null){
				foreach(GameObject crumb in breadcrumbs){
					if(crumb && ai.InRange(crumb.transform.position, ai.visionDistance)){
						if(Physics.Linecast(ai.transform.position, crumb.transform.position, out hit, ~(ai.waypointLayer))){
							if(hit.collider.tag == breadcrumbString) {
								Vector3 diff = crumb.transform.position - ai.FollowingPlayer.position; 
								float curDistance = diff.sqrMagnitude; 
								if (curDistance < distance) {
									ai.Breadcrumb = crumb.transform;
									distance = curDistance;
								}
							}
						}
					}
				}
			}         
			return ai.Breadcrumb;
		}
		
		/*
			Here we're looking for another Ai that is currently following a player.
			If this Ai cannot see a player but it sees another Ai who is following the player (out of our line of sight) then return the Ai to follow
			If the Ai is out of our line of sight or no longer sees a player then look for another Ai
			We set a radius around us and find all Enemy Ai. We cast a line to their position.
			If the ray hits something, then return null, if it doesn't hit anything, then find the closest Ai and return that Ai.
		*/
		
		public static Transform Ai_FindAi(this Ai ai){
			if(ai.FollowingAi){
				if(ai.InRange(ai.FollowingAi.position, ai.visionDistance)){
					if(Physics.Linecast(ai.transform.position, ai.FollowingAi.position, out hit, ~(ai.breadcrumbLayer | ai.waypointLayer))){
						if(hit.collider.tag == enemyString && hit.collider.gameObject.GetComponent<Ai>().visionState == Ai.VISION_STATE.CanSeePlayer ||
						   hit.collider.tag == enemyString && hit.collider.gameObject.GetComponent<Ai>().visionState == Ai.VISION_STATE.CanSeeBreadcrumb){
								return ai.FollowingAi;
						} else {
							ai.FollowingAi = null;
						}
					} else {
						ai.FollowingAi = null;
					}
				} else {
					ai.FollowingAi = null;
				}
			} else {
				float distance = Mathf.Infinity;
				float radius = ai.visionDistance/2;
				if(!ai._HasVision){
					radius = 10000; // If the Ai doesn't use vision, use a radius of 10000
				}
				Collider[] colliders = Physics.OverlapSphere(ai.transform.position,radius, ai.enemyLayer);
				foreach(Collider collider in colliders){
					if(collider.tag == enemyString && collider.gameObject.GetComponent<Ai>().visionState == Ai.VISION_STATE.CanSeePlayer ||
					   collider.tag == enemyString && collider.gameObject.GetComponent<Ai>().visionState == Ai.VISION_STATE.CanSeeBreadcrumb){
						if(!Physics.Linecast(ai.transform.position,collider.transform.position, out hit, ~(ai.breadcrumbLayer | ai.waypointLayer | ai.enemyLayer))){
							Vector3 dir = collider.transform.position - ai.transform.position;
							float dist = dir.sqrMagnitude;
							if(dist < distance){
								ai.FollowingAi = collider.transform;
								distance = dist;
							}
						}
				 	}
				}
			}
			return ai.FollowingAi;
		}
		
		/*
			Here we are doign the same thing as the above FindAi, however this is a 2nd tier.
			The only difference is we're looking for an Ai that is following another ai that is following the player (thats a mouth full)
		*/
		public static Transform Ai_FindAiTierTwo(this Ai ai){
			if(ai.FollowingAi){
				if(ai.InRange(ai.FollowingAi.position, ai.visionDistance)){
					if(Physics.Linecast(ai.transform.position, ai.FollowingAi.position, out hit, ~(ai.breadcrumbLayer | ai.waypointLayer))){
						if(hit.collider.tag == enemyString && hit.collider.gameObject.GetComponent<Ai>().visionState == Ai.VISION_STATE.CanSeeFollowAi){
							return ai.FollowingAi;
						} else {
							ai.FollowingAi = null;
						}
					} else {
						ai.FollowingAi = null;
					}
				} else {
					ai.FollowingAi = null;
				}
			} else {
				float distance = Mathf.Infinity;
				float radius = ai.visionDistance/2;
				if(!ai._HasVision){
					radius = 10000; // If the Ai doesn't use vision, use a radius of 10000
				}
				Collider[] colliders = Physics.OverlapSphere(ai.transform.position,radius, ai.enemyLayer);
				foreach(Collider collider in colliders){
					if(collider.tag == enemyString && collider.gameObject.GetComponent<Ai>().visionState == Ai.VISION_STATE.CanSeeFollowAi){
						if(!Physics.Linecast(ai.transform.position, collider.transform.position, out hit, ~(ai.breadcrumbLayer | ai.waypointLayer | ai.enemyLayer))){
							Vector3 dir = collider.transform.position - ai.transform.position;
							float dist = dir.sqrMagnitude;
							if(dist < distance){
								ai.FollowingAi = collider.transform;
								distance = dist;
							}
						}
					}
				}
			}
			return ai.FollowingAi;
		}
		
		/*
			Here we're finding a waypoint to patol to. If we already have a waypoint and it's not being blocked or we haven't reached it
			then return the current waypoint we're using.
			If something is blocking it or we've reached the waypoint then find a new one.
			We'll cast a line to all available waypoints, if the line hits the way point then we add it to a list of waypoints
			Now we'll randomly choose a waypoint and return it's position
		*/
		public static Transform Ai_FindWaypoint(this Ai ai){
			if(ai.Waypoint){
				if(Physics.Linecast(ai.transform.position, ai.Waypoint.position, out hit, ~(ai.enemyLayer))){
					if(hit.collider.tag == waypointString){
						Vector3 waypointPos = ai.Waypoint.position;
						waypointPos.y = ai.transform.position.y;  /* Todo: Modify this for Ground units only */
						if(Vector3.Distance(ai.transform.position, waypointPos) < 0.5f) {
							ai.Waypoint = null;
						} else {
							return ai.Waypoint;
						}
					} else {
						ai.Waypoint = null;
					}
				} else {
					ai.Waypoint = null;
				}
			}
			List<GameObject> selectWaypoint = new List<GameObject>();
			if(waypoints != null){
				foreach(GameObject waypoint in waypoints){
					if(waypoint && Physics.Linecast(ai.transform.position, waypoint.transform.position, out hit, ~(ai.enemyLayer))){
						if(hit.collider.tag == waypointString){
							selectWaypoint.Add(waypoint);
						}
					}
				}
			}
			int rand = Random.Range(0, selectWaypoint.Count);
			for(int i =0; i < selectWaypoint.Count; i++){
				if( i == rand){
					ai.Waypoint = selectWaypoint[i].transform;
					return ai.Waypoint;
				}
			}
			return null;
		}
	}
}