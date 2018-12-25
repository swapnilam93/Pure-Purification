using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBallSlider : MonoBehaviour {

	public GameObject ballBeam;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float sliderMag = PhidgetController.Singleton.sliderPos;
		ballBeam.transform.rotation = Quaternion.Euler(180f, 0f, 90f * sliderMag -45f);
	}
}
