  //****************************************//
 //* Thank you for buying Breadcrumb Ai!! *//
//****************************************//
//
// This Ai is designed for Unity Free and does not use tools like NavMesh. The system allows enemies to walk around walls, avoid walls, follow closest player,
//	as well as follow other enemies who see a player that it may not see. The Ai can also wander around as well as patrol the level based on your Waypoints.
//
  ///////////////////
 //* TEST IT OUT *//
///////////////////
//
// Webplayer Demo: http://www.zerologics.com/breadcrumbaidemo/
//
  //////////////////////
 //* THINGS TO KNOW *//
//////////////////////
//
//	This system requires certain Tags and Layers. Make sure you have these set up.
//
//		 TAGS:	   	 	Player 	<-- All players must have this tag
//					     Enemy	<-- All enemies must have this tag
//					Breadcrumb	<-- Breadcrumb Prefab must have this tag
//					  Waypoint	<-- Waypoint Prefab must have this tag
//
//	   LAYERS:   8: Player		<-- All players must have this layer
//				 9: Enemy		<-- All enemies must have this layer
//				10: Breadcrumb	<-- Breadcrumb Prefab must have this layer
//				11: Waypoint	<-- Waypoint Prefab must have this layer
//
//	Ai uses different States so that you can add animation and other scripts to your enemy
//
//	These public variables can be called through the Ai script
//
//		    LIFE_STATE = lifeState
//		  VISION_STATE = visionState
//		MOVEMENT_STATE = moveState
//		  ATTACK_STATE = attackState
//
//		LIFE_STATEs
//			- IsAlive
//			- IsDazed (coming soon)
//			- IsDead
//			- IsInvincible
//
//		VISION_STATEs
//			- CanSeeNothing
//			- CanSeePlayer
//			- CanSeeBreadcrumb
//			- CanSeeFollowAi
//			- CanSeeFollowAiTierTwo
//			- CanSeeWaypoint
//
//		MOVEMENT_STATEs
//			- IsIdle
//			- IsFollowingPlayer
//			- IsFollowingBreadcrumb
//			- IsFollowingAi
//			- IsFollowingAiTierTwo
//			- IsWandering
//			- IsPatrolling
//
//		ATTACK_STATEs
//			- CanNotAttack
//			- CanAttackPlayer
//			- CanAttackOther (coming soon)
//
  /////////////////////////
 //* ANIMATION EXAMPLE *//
/////////////////////////
//
//	Ai ai;
//
// 	void Start()
//	{
//		ai = GetComponent<Ai>();
//	}
//
//	void Update()
//	{
//		if(ai.moveState == Ai.MOVEMENT_STATE.IsFollowingPlayer)
//		{
//			//Animation
//		}
//	}
//
  ////////////////////////////
 //* HOW TO SETUP AN ENEMY *//
////////////////////////////
//
// STEP 1:	Make sure your project has the above listed Tags and Layers.
//
// STEP 2:	Create a new scene and place a player and an enemy capsule, or use whatever model you have. Also place something to walk on. ;)
//			*Enemy must have a Rigidbody with frozen rotations.
//			** Player must have Tag "Player" and Later 8: "Player"
//			*** Enemy must have Tag "Enemy" and Layer 9: "Enemy"
//
// STEP 3:	Use your existing player movement script, if not then you can base one off the Demo's code in the Examples folder
//			Scripts located under /BreadcrumbAi/Examples/Demo/Scripts
//			* these may require some tinkering since they were specific to the demo scene.
//			** If you need a hand with player movement, You can check out my Starter Kit Asset using the link below
//				Starter Kit: https://www.assetstore.unity3d.com/en/#!/content/18071
//
// STEP 4:	On your player model add the script called 'Breadcrumbs'
//			Select Add Component > Scripts > BreadcrumbAi > Breadcrumbs
//
// STEP 7: 	Set desired breadcrumb amount and rates. For now use the following.
//				Amount: 10
//			Spawn Rate: 0.2
//			Clear Rate: 0.3
//
// STEP 8:	Go to your Enemy model and add the Ai script.
//			Select Add Component > Scripts > BreadcrumbAi > Ai
//
// STEP 9:	Choose your desired options for your AI. The options are easy to use and customizable.
//				*Note: Try not to have anything set to 0, it can cause issues. If need be you can use 0.01, etc.
//
// STEP 10:	Place a few Waypoint prefabs on your level.
//			Waypoints located under /BreadcrumbAi/Ai/Prefabs
//
// STEP 11: PLAY :)
//
// STEP 12: ISSUES? CONTACT ME! (info below)
//
  //////////////
 //* VIDEOS *//
//////////////
//
//	How to Use: http://youtu.be/cyvdAYOxnqg
//
  /////////////////////////////////////////
 //* Not so frequently asked questions *//
/////////////////////////////////////////
//
// 1.)	My Ai prefab or Breadcrumb prefab aren't updating all my options all the time.
//			A)  If this happens, press the cog wheel on the script and select RESET and reinput your options.
//					*NOTE: A fix is still being looked at for this. If you have any suggestions, please e-mail me. (info below)
//
// 2.)	Sometimes my Ai can see through walls... Why is that?
//			A)  You may notice the Debug.DrawLine is being drawn through walls at times. I'm not exactly sure why they are being drawn
//				but as far as I can tell, it's drawing to the wrong location. Currenty I've not had any enemies running into walls.
//
// 3.)	Breadcrumbs aren't being placed, Ai isn't following breadcrumbs, nothing is working.
//			A)	Be sure to check your Ai / Breadcrumb scripts, if it didn't properly save your settings you may notice that
//				certain fields aren't right, such as Breadcrumb Spawn Rate being 0. Reset the script and try again.
//
// 4.)	Can I modify the scripts in any way?
//			A)	Totally, you can do whatever you want to the script, even the editor script. Just remember that when an update comes out and
//				you get the update, your changes to the script won't stay.
//
// 5.) 	I have more questions and problems but this FAQ didn't help me at all.
//		A) Send me an e-mail or tweet, find that information below! :)
//
  ///////////////
 //* CONTACT *//
///////////////
//
// E-Mail: mike.desjardins@outlook.com
// Twitter: @ZeroLogics
// Site: www.zerologics.com
//
// I'm fairly active on Twitter and through e-mail, so I'm not too hard to get a hold of.
//
//
//
// Thanks you and enjoy!!
//