using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidDropper : MonoBehaviour {

	public GameObject coke;
	public GameObject poison;

	public Transform startPoint;
	
	public Animator animator;
	private AudioSource audioSource;

	GameObject dropLiquid;
	bool pressed;
	bool released;

	void Awake () {
		//animator = this.gameObject.GetComponent<Animator>();
		audioSource = this.gameObject.GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		dropLiquid = Instantiate(coke, startPoint.position, Quaternion.Euler(0f, 0f, 0f));
		pressed = false;
		released = true;
		//Debug.Log(coke.transform.localScale);
	}
	
	// Update is called once per frame
	void Update () {
		if (dropLiquid == null || dropLiquid.tag == "Splash") {
			dropLiquid = GenerateLiquid();
			//StartCoroutine(DestroyLiquid(dropLiquid));
		}
		if (pressed) {
			if (!PhidgetController.Singleton.buttonPressed) {
				if (!released) {
					animator.SetTrigger("Idle");
				}
				released = true;
				pressed = false;
			}
		}
		if (PhidgetController.Singleton.buttonPressed) {
			if (!pressed) {
				animator.SetTrigger("Compress");
				if (dropLiquid != null ) {
					dropLiquid.GetComponent<Rigidbody>().useGravity = true;
					dropLiquid.GetComponent<Rigidbody>().AddForce(0f, -100f, 0f);
				}
			}
			pressed = true;
			released = false;
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			animator.SetTrigger("Compress");
			if (dropLiquid != null ) {
				dropLiquid.GetComponent<Rigidbody>().useGravity = true;
				dropLiquid.GetComponent<Rigidbody>().AddForce(0f, -100f, 0f);
			}
		} else if (Input.GetKeyUp(KeyCode.Space)){
			animator.SetTrigger("Idle");
		}
	}

	public GameObject GenerateLiquid() {
		float random1 = Random.Range(0f, 1f);
		float random2 = Random.Range(0f, 1f);
		if ((random1 + random2)/2 > 0.5f)
			return Instantiate(coke, startPoint.position, Quaternion.Euler(0f, 0f, 0f));
		else
			return Instantiate(poison, startPoint.position, Quaternion.Euler(0f, 0f, 0f));
		//Debug.Log("gen");
	}

	IEnumerator DestroyLiquid(GameObject dropLiquid) {
		yield return new WaitForSeconds(2f);
		if (dropLiquid != null)
			GameObject.Destroy(dropLiquid);
	}
}
