using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour {

    public Transform movingPlatform;
    public Transform position1;
    public Transform position2;

    public Vector3 newPosition;
    public string currentState;
    float smooth;
    float resetTime;

	// Use this for initialization
	void Start () {
        smooth = .7f;
        resetTime = 5f;
        ChangeTarget();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, Time.deltaTime * smooth);
	}

    void ChangeTarget()
    {
        if(currentState == "Move to position 1")
        {
            currentState = "Move to position 2";
            newPosition = position2.position;
        }
        else
        {
            currentState = "Move to position 1";
            newPosition = position1.position;
        }
        
        Invoke("ChangeTarget", resetTime);
    }
}
