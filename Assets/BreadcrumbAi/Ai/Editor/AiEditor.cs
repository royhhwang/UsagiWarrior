using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace BreadcrumbAi{
	[CustomEditor(typeof(Ai))]
	public class AiEditor : Editor {
	
		private Ai ai;
		private Texture2D iconType, iconAvoid, iconAiOptions, iconActions, iconHealth, iconFollow, iconWander, iconPatrol, iconSpeed, iconVision;
		static private bool _showAvoid, _showWander, _showPatrol, _showSpeeds, _showMain, _showActions, _showFollow, _showVision, _showEnemyType, _showHealth;
		private string  strIconPath = "Assets/BreadcrumbAi/Ai/GUI/",
						strType   = "  ENEMY TYPE OPTIONS",
						strMain   = "  Ai OPTIONS",
						strActions = "  ACTIONS",
						strHealth = "  HEALTH OPTIONS",
						strVision = "  VISION OPTIONS",
						strFollow = "  FOLLOW OPTIONS",
						strSpeed  = "  SPEED OPTIONS",
						strWander = "  WANDER OPTIONS",
						strPatrol = "  PATROL OPTIONS",
						strAvoid  = "  AVOID OPTIONS";
			
		private GUISkin skin;
		private bool _initialized;
	
		public override void OnInspectorGUI()
		{
			_Initialize();
			GUI.skin = skin;
			
			if(!ai.transform.GetComponent<Rigidbody>()){
				EditorGUILayout.HelpBox("Note: Rigidbody component is required!", MessageType.Info);
			} else {
				if(ai.transform.GetComponent<Rigidbody>().constraints != RigidbodyConstraints.FreezeRotation){
					EditorGUILayout.HelpBox("Note: Rigidbody Rotation Constraints must be frozen!", MessageType.Info);
				}
			}
			
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent(strType, iconType))){
				_showEnemyType = !_showEnemyType;
			}
			EditorGUILayout.Space();
			if(_showEnemyType){
				EditorGUILayout.BeginHorizontal();
				iconType = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_type_pressed.png", typeof(Texture2D));
							
				ai._IsMelee = EditorGUILayout.Toggle("Melee", ai._IsMelee, "Toggle");
				if(ai._IsMelee){
					ai._IsRanged = false;
				}
				ai._IsRanged = EditorGUILayout.Toggle("Ranged", ai._IsRanged, "Toggle");
				if(ai._IsRanged){
					ai._IsMelee = false;
				}
				EditorGUILayout.EndHorizontal();
				
				EditorGUILayout.BeginHorizontal();
				ai._IsGround = EditorGUILayout.Toggle("Ground", ai._IsGround, "Toggle");
				if(ai._IsGround){
					ai._IsAir = false;
				}
				ai._IsAir = EditorGUILayout.Toggle("Air", ai._IsAir, "Toggle");
				EditorGUILayout.EndHorizontal();
				if(ai._IsAir){
					ai._IsGround = false;
					ai._CanHover = false;
					if(ai.GetComponent<Rigidbody>().useGravity){
						EditorGUILayout.HelpBox("Note: Air enemies should have Gravity turned off.", MessageType.Info);
					}
				}
				
				if(ai._IsGround){
					ai._CanHover =  EditorGUILayout.Toggle("Hover?", ai._CanHover, "Toggle");
				}
				if(ai._CanHover){
					if(!ai.Hover){
						GameObject newHover = new GameObject();
						newHover.transform.position = ai.transform.position;
						newHover.transform.rotation = ai.transform.rotation;
						newHover.transform.parent = ai.transform;
						newHover.name = "Hover";
						ai.Hover = newHover.transform;
					}
					ai.hoverHeight = EditorGUILayout.Slider("Hover Height", ai.hoverHeight,0,100);
					ai.hoverForce = EditorGUILayout.Slider("Hover Force", ai.hoverForce, 0, 100);
				} else {
					if(ai.Hover){
						DestroyImmediate(ai.Hover.gameObject);
						ai.Hover = null;
					}
				}
				
			} else {
				iconType = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_type.png", typeof(Texture2D));
			}
			
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent(strMain, iconAiOptions))){
				_showMain = !_showMain;
			}
			EditorGUILayout.Space();
			if(_showMain){
				iconAiOptions = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_ai_options_pressed.png", typeof(Texture2D));
				ai._CanFollowPlayer = EditorGUILayout.Toggle("Enable Follow Player", ai._CanFollowPlayer, "Toggle");
				EditorGUILayout.BeginHorizontal();
				ai._CanFollowBreadcrumbs = EditorGUILayout.Toggle("Enable Follow Breadcrumbs", ai._CanFollowBreadcrumbs, "Toggle");
				if(ai._CanFollowBreadcrumbs){
					EditorGUILayout.HelpBox("Note: Breadcrumb Script required on Player!", MessageType.Info);
				}
				EditorGUILayout.EndHorizontal();
				ai._CanFollowAi = EditorGUILayout.Toggle("Enable Follow Other Ai", ai._CanFollowAi, "Toggle");
				ai._CanWander = EditorGUILayout.Toggle("Enable Wander", ai._CanWander, "Toggle");
				ai._CanPatrol = EditorGUILayout.Toggle("Enable Patrol", ai._CanPatrol, "Toggle");
				ai._HasVision = EditorGUILayout.Toggle("Enable Vision", ai._HasVision, "Toggle");
				ai._HasAvoidance = EditorGUILayout.Toggle("Enable Avoidance", ai._HasAvoidance, "Toggle");
			} else {
				iconAiOptions = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_ai_options.png", typeof(Texture2D));
			}
			
			// ACTIONS MENU
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent(strActions, iconActions))){
				_showActions = !_showActions;
			}
			EditorGUILayout.Space();
			if(_showActions){
				iconActions = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_ai_options_pressed.png", typeof(Texture2D));
				if(ai._IsRanged){
					ai._CanFlee = EditorGUILayout.Toggle("Enable Flee", ai._CanFlee, "Toggle");
				}
				ai._CanJump = EditorGUILayout.Toggle("Enable Jump", ai._CanJump, "Toggle");
				if(ai._CanJump){
					ai.jumpDistance = EditorGUILayout.Slider("Jump Distance", ai.jumpDistance, 0.1f,20f);
					ai.jumpForce = EditorGUILayout.Slider("Jump Force", ai.jumpForce, 0.1f,50f);
					if(!ai.JumpDetector){
						GameObject newJump = new GameObject();
						newJump.transform.rotation = ai.transform.rotation;
						newJump.transform.position = ai.transform.position + (-ai.transform.up * 0.5f);
						newJump.transform.parent = ai.transform;
						newJump.name = "JumpDetector";
						ai.JumpDetector = newJump.transform;
					}
				} else {
					if(ai.JumpDetector){
						DestroyImmediate(ai.JumpDetector.gameObject);
						ai.JumpDetector = null;
					}
				}
				
				ai._CanLongJump = EditorGUILayout.Toggle("Enable Long Jump", ai._CanLongJump, "Toggle");
				if(ai._CanLongJump){
					ai.longJumpForce = EditorGUILayout.Slider("Long Jump Force", ai.longJumpForce, 0.1f,50f);
					if(!ai.LongJumpDetector){
						GameObject newJump = new GameObject();
						newJump.transform.rotation = ai.transform.rotation;
						newJump.transform.position = ai.transform.position + (ai.transform.forward * 5);
						newJump.transform.parent = ai.transform;
						newJump.name = "LongJumpDetector";
						ai.LongJumpDetector = newJump.transform;
					}
				} else {
					if(ai.LongJumpDetector){
						DestroyImmediate(ai.LongJumpDetector.gameObject);
						ai.LongJumpDetector = null;
					}
				}
				
			} else {
				iconActions = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_ai_options.png", typeof(Texture2D));
			}
			
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent(strHealth, iconHealth))){
				_showHealth = !_showHealth;
			}
			EditorGUILayout.Space();
			if(_showHealth){
				iconHealth = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_health_pressed.png", typeof(Texture2D));
				ai.Health = EditorGUILayout.Slider("Enemy Health", ai.Health, 0.0f, 10000.0f);
				ai._IsInvincible = EditorGUILayout.Toggle("Invincibility", ai._IsInvincible, "Toggle");
			} else {
				iconHealth = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_health.png", typeof(Texture2D));
			}
			
			
			if(ai._HasVision){
				EditorGUILayout.Space();
				if(GUILayout.Button(new GUIContent(strVision, iconVision))){
					_showVision = !_showVision;
				}
				EditorGUILayout.Space();
				if(_showVision){
					iconVision = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_vision_pressed.png", typeof(Texture2D));
					ai.visionDistance = EditorGUILayout.Slider("Vision Distance",ai.visionDistance,0.0f,200.0f);
					ai._HasFrontVision = EditorGUILayout.Toggle("Enable Front Vision", ai._HasFrontVision, "Toggle");
				} else {
					iconVision = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_vision.png", typeof(Texture2D));
				}
			}
	
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent(strFollow, iconFollow))){
				_showFollow = !_showFollow;
			}
			EditorGUILayout.Space();
			if(_showFollow){
				iconFollow = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_follow_pressed.png", typeof(Texture2D));
				ai.followDistance = EditorGUILayout.Slider("Follow Distance", ai.followDistance, 0.0f, 100.0f);
				ai.attackDistance = EditorGUILayout.Slider("Attack Distance", ai.attackDistance, 0.0f, 100.0f);
				if(ai._CanFollowAi){
					ai.otherAiDistance = EditorGUILayout.Slider("Follow Ai Distance", ai.otherAiDistance, 0.0f, 100.0f);
				}
			} else {
				iconFollow = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_follow.png", typeof(Texture2D));
			}
			
			
			
			EditorGUILayout.Space();
			if(GUILayout.Button(new GUIContent(strSpeed, iconSpeed))){
				_showSpeeds = !_showSpeeds;
			}
			EditorGUILayout.Space();
			if(_showSpeeds){
				iconSpeed = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_speed_pressed.png", typeof(Texture2D));
				if(ai._CanFollowPlayer){
					ai.followSpeed = EditorGUILayout.Slider("Follow Speed", ai.followSpeed,0.0f,100.0f);
				}
				if(ai._CanWander || ai._CanPatrol){
					ai.wanderSpeed = EditorGUILayout.Slider("Wander Speed",ai.wanderSpeed,0.0f,100.0f);
				}
				if(ai._CanPatrol){
					ai.patrolSpeed = EditorGUILayout.Slider("Patrol Speed", ai.patrolSpeed,0.0f,100.0f);
				}
				ai.rotationSpeed = EditorGUILayout.Slider("Rotation Speed", ai.rotationSpeed,0.0f,10.0f);
			} else {
				iconSpeed = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_speed.png", typeof(Texture2D));
			}
	
	
				
			if(ai._CanWander){
				EditorGUILayout.Space();
				if(GUILayout.Button(new GUIContent(strWander, iconWander))){
					_showWander = !_showWander;
				}
				EditorGUILayout.Space();
				if(_showWander){
					iconWander = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_wander_pressed.png", typeof(Texture2D));
					ai._CanWanderAnywhere = EditorGUILayout.Toggle("Can Wander Anywhere", ai._CanWanderAnywhere);
					ai.wanderTimeLimit = EditorGUILayout.Slider("Wander Time Limit", ai.wanderTimeLimit,1.0f,120.0f);
					ai.wanderTimeRate = EditorGUILayout.Slider("Wander Time Rate", ai.wanderTimeRate,0.0f,60.0f);
					ai.wanderDistance = EditorGUILayout.Slider("Wander Distance", ai.wanderDistance,1.0f,50.0f);
					
				} else {
					iconWander = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_wander.png", typeof(Texture2D));
				}
			}
			
			if(ai._CanPatrol){
				EditorGUILayout.Space();
				if(GUILayout.Button(new GUIContent(strPatrol,iconPatrol))){
					_showPatrol = !_showPatrol;
				}
				EditorGUILayout.Space();
				if(_showPatrol){
					iconPatrol = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_wander_pressed.png", typeof(Texture2D));
				} else {
					iconPatrol = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_wander.png", typeof(Texture2D));
				} 
			}
			
			
			if(ai._HasAvoidance){
				EditorGUILayout.Space();
				if(GUILayout.Button(new GUIContent(strAvoid, iconAvoid))){
					_showAvoid = !_showAvoid;
				}
				EditorGUILayout.Space();
				if(_showAvoid){
					iconAvoid = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_avoid_pressed.png", typeof(Texture2D));
					ai.avoidDistance = EditorGUILayout.Slider("Avoid Distance", ai.avoidDistance,0.01f,20.0f);
					ai.avoidSpeed = EditorGUILayout.Slider("Avoidance Speed", ai.avoidSpeed,0.01f,500.0f);
					
					ai._HasEdgeAvoidance = EditorGUILayout.Toggle("Enable Edge Avoidance", ai._HasEdgeAvoidance, "Toggle");
					if(ai._HasEdgeAvoidance){
						ai.edgeDistance = EditorGUILayout.Slider("Edge Distance", ai.edgeDistance, 0.01f, 10.0f);
						if(!ai.Edge){
							GameObject newEdge = new GameObject();
							newEdge.transform.rotation = ai.transform.rotation;
							newEdge.transform.position = ai.transform.position + ai.transform.forward;
							newEdge.transform.parent = ai.transform;
							newEdge.name = "EdgeDetector";
							ai.Edge = newEdge.transform;
						}
					} else {
						if(ai.Edge){
							DestroyImmediate(ai.Edge.gameObject);
							ai.Edge = null;
						}
					}
				} else {
					iconAvoid = (Texture2D)AssetDatabase.LoadAssetAtPath(strIconPath + "icon_avoid.png", typeof(Texture2D));
				}
			}
		}
		
		private void _Initialize(){
			if(!_initialized){
				ai = (Ai) target;
				skin = (GUISkin)AssetDatabase.LoadAssetAtPath(strIconPath + "BreadcrumbAiGUISkin.guiskin", typeof(GUISkin));
				_initialized = true;
			}
			if(!ai._IsMelee && !ai._IsRanged){
				ai._IsMelee = true;
			}
			if(!ai._IsGround && !ai._IsAir){
				ai._IsGround = true;
			}
		}
	}
}