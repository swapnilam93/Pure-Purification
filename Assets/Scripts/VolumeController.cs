using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeController : MonoBehaviour {

	public AudioListener audioListener;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float sliderMag = PhidgetController.Singleton.sliderPos;
		AudioListener.volume = sliderMag;
	}
}
