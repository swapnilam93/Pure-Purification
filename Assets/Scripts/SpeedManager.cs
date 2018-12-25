using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedManager : MonoBehaviour {

	public ConveyorBelt conveyorBelt;
	public CokeGenerator cokeGenerator;

	public AudioSource[] bgms;

	private bool playFast;
	bool isMusicMode = false;
	private float timer;

	private void Awake() {
		
	}

	// Use this for initialization
	void Start () {
		isMusicMode = GameController.Singleton.isMusicMode;
		playFast = false;
		bgms[0].Play();
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (!isMusicMode) {
			if (timer + cokeGenerator.startSpeed > cokeGenerator.levelTime) {
				/*cokeGenerator.levelTime *= 1.5f;
				if (cokeGenerator.repeatRate > 0.55f) {
					cokeGenerator.repeatRate *= 0.8f;
				}
				if (conveyorBelt.speed < 2.0f) {
					conveyorBelt.speed *= 1.1f;
				}*/
				cokeGenerator.levelTime += 30f;
				cokeGenerator.repeatRate /= 1.8f;
				conveyorBelt.speed *= 1.2f;
			}
			if (!bgms[0].isPlaying) {
				GameOver();
			}
			/*if (conveyorBelt.speed > 2.0f && !playFast) {
				bgms[0].Stop();
				//bgms[0].mute = true;
				bgms[1].Play();
				playFast = true;
			}*/
		}
		
	}

	void GameOver() {
		GameObject.Find("GameManager").GetComponent<GameController>().GameOver();
	}
}
