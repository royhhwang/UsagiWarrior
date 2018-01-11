using UnityEngine;
using System.Collections;

public class DemoDestroyExplosion : MonoBehaviour {

	public GameObject pointLight;

	void Start () {
		Destroy(pointLight, 0.1f);
		Destroy(gameObject, 1);
	}
}