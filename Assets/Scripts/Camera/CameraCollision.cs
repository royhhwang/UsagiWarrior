using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour {

    public float minDistance, maxDistance, smooth, distance, zoomSpeed;
    Vector3 dollyDir;
    public Vector3 dollyDirAdjusted;

	// Use this for initialization
	void Awake () {
        dollyDir = transform.localPosition.normalized;
        distance = transform.localPosition.magnitude;

        minDistance = 3f;
        maxDistance = 4f;
        smooth = 5f;
        zoomSpeed = 3f;
        
	}
	
	// Update is called once per frame
	void Update () {

        //Camera Zoom in/out Feature
        maxDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        if (maxDistance < minDistance)
        {
            maxDistance = minDistance;
        }

        if (maxDistance > 10f)
        {
            maxDistance = 10f;
        }

        //Camera Object Collision and reposition feature
        Vector3 desiredCameraPos = transform.parent.TransformPoint(dollyDir * maxDistance);
        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, desiredCameraPos, out hit)) //If there is an object between player and max distance of camera
        {   
            distance = Mathf.Clamp((hit.distance * 0.9f), minDistance, maxDistance);
        }

        else
        {
            distance = maxDistance;
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * distance, Time.deltaTime * smooth);
		
	}
}
