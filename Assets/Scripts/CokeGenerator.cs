using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CokeGenerator : MonoBehaviour {

	public float startSpeed;
	public float repeatRate;
	public GameObject coke;
	//public GameObject poison;
	public Transform startPoint;
	public float levelTime;
	private Animator animator;
	bool AutoStartGenerate = true;
	public bool generate;

	void Awake () {
		animator = this.gameObject.GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
		AutoStartGenerate = !GameController.Singleton.isMusicMode;
		generate = true;
		if (AutoStartGenerate)
			Invoke("GenerateCoke", startSpeed);
		//Debug.Log(coke.transform.localScale);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GenerateCoke() {
		float random1 = Random.Range(0f, 1f);
		float random2 = Random.Range(0f, 1f);
		if ((random1 + random2)/2 > 0.3f) {
			Instantiate(coke, startPoint.position, Quaternion.Euler(0f, 120f, 0f));
			animator.SetTrigger("CokeGen");
		}
		else {
			//drop
			//Instantiate(poison, startPoint.position, Quaternion.Euler(0f, 120f, 0f));
		}
		//Debug.Log("gen");
		if (generate)
			Invoke("GenerateCoke", repeatRate);
	}

	public GameObject GenerateRemoteCoke() {
		var can = Instantiate(coke, startPoint.position, Quaternion.Euler(0f, 120f, 0f));
		can.GetComponent<BeatControlledCan>().enabled = true;
		animator.SetTrigger("CokeGen");
		return can;
	}
}
