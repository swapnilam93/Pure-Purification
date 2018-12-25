using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour {

	public GameObject belt;
	public GameObject surface;
	public Transform endpoint;
	public float speed;
	public float visualSpeedScalar;
	private float currentScroll;
	//public AudioClip canDrop;
	private Vector3 direction;
	//private Rigidbody rigidbody;
	//private AudioSource audioSource;
	private CokeCollector cokeCollector;
	public bool CokeMoveByBelt = true;

	private void Awake() {
		//rigidbody = this.GetComponent<Rigidbody>();
		//audioSource = this.GetComponent<AudioSource>();
		cokeCollector = GameObject.Find("Collector").GetComponent<CokeCollector>();
	}

	/*private void OnTriggerEnter(Collider other) {
		if (other.tag == "Coke" || other.tag == "Poison") {
			//using transform move
			audioSource.PlayOneShot(canDrop);
		}
	}*/

	void Update() {
		currentScroll = currentScroll - Time.deltaTime*speed*visualSpeedScalar;
    	surface.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0, currentScroll);
	}

	void OnTriggerStay(Collider other) {
		bool shouldMove = false;
		if (other.CompareTag("Splash")) {
			shouldMove = true;
		}
		if (other.CompareTag("Soda") || other.CompareTag("Coke") || other.CompareTag("Poison")) {
			//using transform move
			if (CokeMoveByBelt) {
				shouldMove = true;
			}
			else {
				// sorry Swapnil
				var controlledCan = other.gameObject.GetComponent<BeatControlledCan>();
				if (controlledCan != null && controlledCan.stage > 1) {
					shouldMove = true;
				}
			}
		}
		if (shouldMove) {
			other.transform.position = Vector3.MoveTowards(other.transform.position, endpoint.position, speed * Time.deltaTime);
		}
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "CokeLiquid") {
			//animator.SetTrigger("Pop");
			//LerpMaterial2to1();
			//GameObject.Destroy(other.gameObject);
			other.gameObject.tag = "Splash";
			other.gameObject.GetComponent<Animator>().SetTrigger("Splash");
			StartCoroutine(splashedLiquid(other.gameObject));
			Debug.Log(cokeCollector);
			cokeCollector.LiquidSpill();

		}
		else if (other.gameObject.tag == "PoisonLiquid") {
			//animator.SetTrigger("Pop");
			//LerpMaterial1to2();
			//GameObject.Destroy(other.gameObject);
			other.gameObject.tag = "Splash";
			other.gameObject.GetComponent<Animator>().SetTrigger("Splash");
			StartCoroutine(splashedLiquid(other.gameObject));
			cokeCollector.LiquidSpill();
		}
		else if (other.gameObject.CompareTag("Soda")) {
			other.gameObject.GetComponent<BeatControlledCan>().isFreeToMove = true;
		}
	}

	IEnumerator splashedLiquid(GameObject liquid) {
		yield return new WaitForSeconds(0.1f);
		liquid.transform.GetChild(0).gameObject.SetActive(true);
		liquid.transform.GetChild(1).gameObject.SetActive(true);
		liquid.transform.GetChild(2).gameObject.SetActive(true);
	}

	/*private void OnCollisionStay(Collision other) {
		//if (other.gameObject.tag == "Coke" || other.gameObject.tag == "Poison") {
			//using force
			direction = transform.right;
			direction = direction * speed;
			Debug.Log(direction);
			other.rigidbody.AddForce(direction, ForceMode.Acceleration);
		//}
	}*/

	/*private void LateUpdate() {
		Vector3 movement = transform.right * speed * Time.deltaTime;

		rigidbody.position -= movement;

		rigidbody.MovePosition(rigidbody.position + movement);
	}*/
}
