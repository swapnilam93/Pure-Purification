using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatControlledCan : MonoBehaviour {
	public bool isFreeToMove = false;
	public Transform Stop1, Stop2;
	public float dspTime1, dspTime2, startDsp;
	public AudioSource audioSource;
	public int stage = 0;
	bool startRecorded = false;
	Vector3 startPos = new Vector3();
	// Use this for initialization
	void Start () {
		if (audioSource == null) {
			enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (isFreeToMove && !startRecorded) {
			startRecorded = true;
			startPos = transform.position;
			startDsp = (float)AudioSettings.dspTime;
		}
		float t;
		Vector3 newPos;
		if (isFreeToMove) {
			float dspTime = (float)AudioSettings.dspTime;
			switch (stage) {
				case 0:
					t = (dspTime - startDsp) / (dspTime1 - startDsp);
					newPos = Vector3.Lerp(startPos, Stop1.position, t);
					newPos.y = transform.position.y;
					transform.position = newPos;
					if (dspTime >= dspTime1) {
						startPos = transform.position;
						startDsp = dspTime;
						stage = 1;
					}
					break;
				case 1:
					t = (dspTime - startDsp) / (dspTime2 - startDsp);
					newPos = Vector3.Lerp(startPos, Stop2.position, t);
					newPos.y = transform.position.y;
					transform.position = newPos;
					if (dspTime >= dspTime2) {
						startPos = transform.position;
						startDsp = dspTime;
						stage = 2;
					}
					break;
				case 2:
					//gameObject.tag = "Soda";
					break;
			}
		}
	}
}
