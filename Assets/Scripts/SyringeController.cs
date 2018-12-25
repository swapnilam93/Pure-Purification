using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyringeController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = PhidgetController.Singleton.accelerometerValRaw;
	}
}
