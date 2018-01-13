using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwing : MonoBehaviour {

    private float swingSpeed;
    private float swingReloadTime;
    private float swingAngle;
    private float normalAngle;
    private GameObject Sword;

	// Use this for initialization
	void Start () {
        swingSpeed = 5f;
        swingReloadTime = 10f;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            Vector3 v = Sword.transform.eulerAngles;
            transform.rotation.eulerAngles.z = Mathf.Lerp(Sword.transform.rotation.eulerAngles.z, swingAngle, Time.deltaTime * swingSpeed);
        }
        else
        {
            Sword.transform.rotation.eulerAngles.z = Mathf.Lerp(Sword.transform.rotation.eulerAngles.z, normalAngle, Time.deltaTime * swingSpeed);
        }
    }
}
