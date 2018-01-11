using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using BreadcrumbAi;

[CustomEditor(typeof(Breadcrumbs))]
public class BreadcrumbEditor : Editor {
	
	private Breadcrumbs crumbs;
	private GUISkin skin;
	private string  strIconPath = "Assets/BreadcrumbAi/Ai/GUI/";
	private bool _initialized;
	
	
	public override void OnInspectorGUI()
	{
		_Initialize();
		GUI.skin = skin;

		EditorGUILayout.Space();
		
		if(crumbs.gameObject.tag != "Player")	EditorGUILayout.HelpBox("Note: Tag must be set to 'Player'", MessageType.Info);
		if(crumbs.gameObject.layer != LayerMask.NameToLayer("Player")) EditorGUILayout.HelpBox("Note: Layer must be set to 'Player'", MessageType.Info);
		
		crumbs._hasUFPS = EditorGUILayout.Toggle("Enable UFPS Compatibility", crumbs._hasUFPS);
		crumbs.breadcrumbAmount = EditorGUILayout.IntSlider("Breadcrumb Amount",crumbs.breadcrumbAmount,1,30);
		GUIContent spawnText = new GUIContent("Spawn Rate", "Breadcrumb spawn rate between 0 and 1");
		crumbs.breadRate = EditorGUILayout.Slider(spawnText,crumbs.breadRate,0,1);
		GUIContent cleanText = new GUIContent("Clear Rate", "Breadcrumb cleaning rate between 0 and 1");
		crumbs.breadCleanRate = EditorGUILayout.Slider(cleanText,crumbs.breadCleanRate,0,1);
		
		if(crumbs.breadCleanRate < crumbs.breadRate){
			EditorGUILayout.HelpBox("Note: Clear Rate should be higher than Spawn Rate", MessageType.Info);
		}
	}
	
	private void _Initialize(){
		if(!_initialized){
			crumbs = (Breadcrumbs) target;
			skin = (GUISkin)AssetDatabase.LoadAssetAtPath(strIconPath + "BreadcrumbAiGUISkin.guiskin", typeof(GUISkin));
			_initialized = true;
		}
	}
}