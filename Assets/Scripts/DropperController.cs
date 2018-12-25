using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropperController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var pos = new Vector3();
		pos = PhidgetController.Singleton.thumbstickPos;
		transform.position = pos;
	}
}
