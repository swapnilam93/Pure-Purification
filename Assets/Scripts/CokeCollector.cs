using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CokeCollector : MonoBehaviour {

	public float waitTime;
	public Text scoreText;
	public Text liquidSpillText;
	public Text destroyCokeText;
	public Text collectionText;
	public CameraChanger cameraChanger;
	public AudioClip successDrop;
	public AudioClip failDrop;
	public int score;
	private AudioSource audioSource;

	private void Awake() {
		audioSource = this.GetComponent<AudioSource>();
	}
	// Use this for initialization
	void Start () {
		score = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter(Collider other) {
		if (other.tag == "Coke") {
			CollectCoke(other);
		}
		else if (other.tag == "Poison") {
			audioSource.PlayOneShot(failDrop);
			CollectPoison(other);
		}
		else if (other.tag == "Soda") {
			audioSource.PlayOneShot(failDrop);
			CollectCan(other);
		}
		else if (other.tag == "Splash") {
			GameObject.Destroy(other.gameObject);
		}
	}

	void CollectCoke (Collider other) {
		if (other.gameObject.GetComponent<CokeShader>().isCapped) {
			audioSource.PlayOneShot(successDrop);
			EnableCollectionText(new Color(0.34f, 0.64f, 0.85f), "+50");
			score += 50;
		} else {
			audioSource.PlayOneShot(failDrop);
			EnableCollectionText(new Color(0.60f, 0.45f, 0.73f), "-20");
			score -= 20;
		}
		StartCoroutine(Destroy(other));
	}

	void CollectPoison (Collider other) {
		if (other.gameObject.GetComponent<CokeShader>().isCrushed) {
			audioSource.PlayOneShot(successDrop);
			EnableCollectionText(new Color(0.80f, 0.50f, 0.32f), "+20");
			score += 20;
		}
		else {
			score -= 30;
			EnableCollectionText(new Color(0.50f, 0.75f, 0.32f, 1f), "-30");
			//StartCoroutine(cameraChanger.Shake(.15f, .05f));
			StartCoroutine(Destroy(other));
		}
	}

	void CollectCan (Collider other) {
		if (other.gameObject.GetComponent<CokeShader>().isCrushed || other.gameObject.GetComponent<CokeShader>().isCapped) {
			score -= 10;
			EnableCollectionText(new Color(0.11f, 0.11f, 0.11f, 1f), "-10");
			//StartCoroutine(cameraChanger.Shake(.15f, .02f));
			StartCoroutine(Destroy(other));
		} else {
			StartCoroutine(Destroy(other));
		}
	}

	void EnableCollectionText(Color color, string text) {
		collectionText.enabled = true;
		collectionText.color = color;
		collectionText.text = text;
		collectionText.GetComponent<Animator>().SetTrigger("Disappear");
	}

	IEnumerator Destroy(Collider coke) {
		scoreText.text = "Score: " + score;
		scoreText.fontSize = 60;
		StartCoroutine(DefaultFont());
		yield return new WaitForSeconds(waitTime);
		collectionText.enabled = false;
		GameObject.Destroy(coke.gameObject);
	}

	IEnumerator DefaultFont() {
		yield return new WaitForSeconds(waitTime);
		//scoreText.color = Color.white;
		scoreText.fontSize = 50;
	}

	public void LiquidSpill() {
		score -= 5;
		scoreText.text = "Score: " + score;
		scoreText.fontSize = 60;
		liquidSpillText.enabled = true;
		liquidSpillText.GetComponent<Animator>().SetTrigger("Disappear");
		StartCoroutine(DisableLiquidSpill());
	}

	IEnumerator DisableLiquidSpill() {
		yield return new WaitForSeconds(waitTime);
		liquidSpillText.enabled = false;
		scoreText.fontSize = 50;
	}

	public void DestroyCokePenalty() {
		score -= 40;
		scoreText.text = "Score: " + score;
		scoreText.fontSize = 60;
		destroyCokeText.enabled = true;
		destroyCokeText.GetComponent<Animator>().SetTrigger("Disappear");
		StartCoroutine(DisableDestroyCoke());
	}

	IEnumerator DisableDestroyCoke() {
		yield return new WaitForSeconds(waitTime);
		destroyCokeText.enabled = false;
		scoreText.fontSize = 50;
	}
}
