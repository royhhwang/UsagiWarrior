using UnityEngine;
using System.Collections;

public class DemoScore : MonoBehaviour {

	public GUISkin skin;
	public int currentScore;
	public bool multiplier;
	private int scorePoint;
	private float timer;
	private float timerLimit = 2.0f;
	
	private void OnGUI(){
		GUI.skin = skin;
		skin.label.fontSize = 24;
		GUI.Label(new Rect(Screen.width-120,0, 100,50), "" + currentScore);
		if(multiplier){
			skin.label.fontSize = 12;
			GUI.Label(new Rect(Screen.width-120,15, 100,50), "x2");
		}
	}
	
	void Update(){
		if(multiplier){
			timer += Time.deltaTime;
			if(timer > timerLimit){
				multiplier = false;
			}
		} else {
			timer = 0.0f;
		}
	}
	
	public void ScorePoint(int score){
		if(!multiplier){
			currentScore += score;
			multiplier = true;
		} else {
			currentScore += score*2;
			timer = 0.0f;
		}
	}
}