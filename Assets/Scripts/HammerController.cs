using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour {

	Vector3 magOffset = new Vector3();
	public float HitMagnitude = 4f;
	public float ZeroThreshold = 0.02f;
	public float HitThreshold = 0.2f;
	public float BadHitThreshold = 0.6f;
	public int HitCoolDown = 10;
	public Transform CapperBody;
	public float CapperGoodEndY = 0.07f;
	public float CapperBadEndY = 0.11f;

	public CameraChanger cameraChanger;
	float originalY = 0;
	int lastHitFrame = -10;
	Queue<float> buffer = new Queue<float>();
	private CokeCollector cokeCollector;

	private void Awake() {
		cokeCollector = GameObject.Find("Collector").GetComponent<CokeCollector>();	
	}

	// Use this for initialization
	void Start () {
		originalY = CapperBody.localPosition.y;
	}
	float MaxVal(float[] a) {
		float ret = 0f;
		foreach (var v in a) {
			if (v > ret)
				ret = v;
		}
		return ret;
	}
	void TriggerNormalHit() {
		RaycastHit hit;
		Physics.Raycast(CapperBody.position, CapperBody.TransformDirection(Vector3.down), out hit, 5f);
		if (hit.transform.gameObject.tag == "Coke" || hit.transform.gameObject.tag == "Poison" || hit.transform.gameObject.tag == "Soda") {
			Debug.Log("Normal Hit");
			hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Cap");
			CokeShader cokeShader = hit.transform.gameObject.GetComponent<CokeShader>();
			cokeShader.CapCan();
			cokeShader.isCapped = true;
		}
	}
	void TriggerBadHit() {
		RaycastHit hit;
		Physics.Raycast(CapperBody.position, CapperBody.TransformDirection(Vector3.down), out hit, 5f);
		if (hit.transform.gameObject.tag == "Coke" || hit.transform.gameObject.tag == "Poison" || hit.transform.gameObject.tag == "Soda") {
			Debug.Log("Bad Hit");
			StartCoroutine(cameraChanger.Shake(.15f, .05f));
			CokeShader cokeShader = hit.transform.gameObject.GetComponent<CokeShader>();
			cokeShader.CapCan();
			hit.transform.gameObject.GetComponent<Animator>().SetTrigger("Crush");
			cokeShader.SplashLiquid();
			cokeShader.isCrushed = true;
			if(hit.transform.gameObject.tag == "Coke")
				cokeCollector.DestroyCokePenalty();
		}
	}
	void Update () {
		if (Input.GetKeyDown(KeyCode.M)) {
			magOffset = PhidgetController.Singleton.magnetValRaw;
		}
		var len = (magOffset - PhidgetController.Singleton.magnetValRaw).magnitude;
        // Debug.Log(len);
        //Debug.Log(PhidgetController.Singleton.forceRaw);
        if (len < ZeroThreshold) {
			len = 0;
		}
		if (len > HitMagnitude) {
			len = HitMagnitude;
		}
		buffer.Enqueue(PhidgetController.Singleton.forceRaw);



		if (buffer.Count > 3) {
			var lastval = buffer.Dequeue();
			if (Time.frameCount - lastHitFrame > HitCoolDown) {
				if (lastval > HitThreshold) {
					if (MaxVal(buffer.ToArray()) > BadHitThreshold) {
						TriggerBadHit();
					}
					else {
						TriggerNormalHit();
					}
					lastHitFrame = Time.frameCount;
				}
			}
		}

		if (Input.GetKeyDown(KeyCode.Return)) {
			TriggerNormalHit();
		} else if (Input.GetKeyDown(KeyCode.RightShift)) {
			TriggerBadHit();
		}
		
		var t = CapperGoodEndY * len / HitMagnitude;
		var tmpLocalPos = new Vector3();
		tmpLocalPos = CapperBody.localPosition;
		tmpLocalPos.y = originalY - t;
		CapperBody.localPosition = tmpLocalPos;

		

		// var t = - (len - HitMagnitude) * (len - HitMagnitude) / HitMagnitude / HitMagnitude + 1;
		// Quaternion q = Quaternion.Euler(0, 0, 90 * t);
		// transform.localRotation = q;
	}
}
