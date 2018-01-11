using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BreadcrumbAi{
	public class Breadcrumbs : MonoBehaviour {
		
		// EDITOR VARIABLES
		public int breadcrumbAmount;
		public float breadRate, breadCleanRate;
		public bool _hasUFPS;
		
		// PRIVATE VARIABLES
		protected GameObject breadCrumbs;
		private Vector3 prevPos;
		private List<GameObject> pooledCrumbs = new List<GameObject>(),
		 						 usedCrumbs = new List<GameObject>();
		private float breadNext = 0.0f,
		  			  breadCleanNext = 0.0f;
		  			  
		private string aiError = " Please go to Edit > Project Settings > Tags and Layers\n" +
								 "Maximize the Tag section and add the proper Tags.\n" +
								 "Maximize the Layers section and add the proper Layers.\n" +
								 "You can find the right Tags and Layers in the ReadMe file\n" +
								 "Go to BreadcrumbAi > Ai > Documentation > ReadMe\n\n";
	
		#if UNITY_EDITOR
		// This places a blue cube in the position of the breadcrumb location in the Editor
		void OnDrawGizmos(){
			Gizmos.color = Color.blue;
			for(int i = 0; i < usedCrumbs.Count; i++){
				Gizmos.DrawCube(usedCrumbs[i].transform.position,usedCrumbs[i].transform.localScale);
			}
		}
		#endif

		void Start (){
			InitPooling();
			if(_hasUFPS) Invoke("InitUFPSCompatibility",0.1F);
		}
		
		void Update(){
			PlaceCrumbs();
			RepoolCrumbs();
		}
		
		/* 
			This is a temp solution to UFPS compatibility. We first look to see if the layer "Player" exists.
			If the layer exists we set the transforms layer to the right int as well as all the children objects.
		*/
		private void InitUFPSCompatibility(){
			int i;
			for(i = 0; i < 32; i++){ // layers range from 0-31
				if(LayerMask.LayerToName(i) == "Player"){
					transform.gameObject.layer = i;
					break;
				}
			}
			if(LayerMask.NameToLayer("Player") == i){
				foreach(Transform child in transform){
					if(!child.tag.Contains("Camera")){
						child.gameObject.layer = i;
						child.gameObject.tag = transform.gameObject.tag;
					}
				}
			} else {
				Debug.Log("ERROR: Player Layer has not been set up.\n" + aiError);
			}
		}
		
		/*
			Breadcrumbs are pooled rather than isntantiated/destroyed
			So here we create breadcrumbs based on the amount you've chosen
			When a breadcrumb is created it becomes Pooled
		*/
		private void InitPooling(){
			breadCrumbs = new GameObject("Breadcrumbs");
			for (int i = 0; i < breadcrumbAmount; i++){
				GameObject newObj = new GameObject();
				newObj.name = "Breadcrumb";
				newObj.AddComponent<BoxCollider>();
				newObj.GetComponent<BoxCollider>().isTrigger = true;
				newObj.layer = LayerMask.NameToLayer("Breadcrumb");
				try{
					newObj.tag = "Breadcrumb";
				} catch(UnityException ex){
					Debug.Log(ex.Message + "\n" + aiError);
				}
				PoolCrumb(newObj);
			}
		}
		
		//This gets a pooled breadcrumb when the player is moving based on the spawn rate
		private void PlaceCrumbs(){
			if(Time.time > breadNext){
				breadNext = Time.time + breadRate;
				if(transform.position != prevPos){
					GetPooledCrumb();
				}
			}
			prevPos = transform.position;
		}
		

		//Repooling is done based on a time limit. When the limit is reached, pool the breadcrumb
		private void RepoolCrumbs(){
			if(Time.time > breadCleanNext){
				breadCleanNext = Time.time + breadCleanRate;
				if(usedCrumbs.Count > 0) {
					GameObject used = usedCrumbs[0];
					usedCrumbs.RemoveAt(0);
					PoolCrumb(used);
				}
			}
		}
		
		/*
			Getting a pooled breadcrumb will take the first object in the list of pooled objects
			It then removes the parent and makes it active while moving it's position to the players position
			We then add it to the used pooled objects list
		*/
		private GameObject GetPooledCrumb (){
			if(pooledCrumbs.Count > 0){
				GameObject pooledObject = pooledCrumbs[0];
				pooledCrumbs.RemoveAt(0);
				pooledObject.transform.parent = null;
				pooledObject.SetActive(true);
				pooledObject.transform.position = transform.position;
				usedCrumbs.Add(pooledObject);
				return pooledObject;	
			}		
			return null;
		}
		
		// Pooling a breadcrumb simply deactivates the gameobject and sets the parent to the pooled object and adds to the pool list
		private void PoolCrumb ( GameObject crumb ){
			crumb.SetActive(false);
			crumb.transform.parent = breadCrumbs.transform;
			pooledCrumbs.Add(crumb);
		}
	}
}