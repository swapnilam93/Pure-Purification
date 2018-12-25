using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LiquidVolumeFX;

public class CokeShader : MonoBehaviour {

	public Material  material1;
	public Material material2;
	public float duration = 2.0F;
    public Renderer canEmptyRend;
	public Renderer canCapRend;
	public Renderer smashedRend;
	public GameObject liquidCoke;
	public GameObject liquidPoison;
	public AudioClip liquidDrop;
	public AudioClip canDrop;
	public AudioClip crushClip;
	public AudioClip capClip;
	public ParticleSystem coke_splash;
	public ParticleSystem poison_splash;
	bool toPoison;
	bool toCoke;
	bool coroutineCalled;

	private Animator animator;
	private AudioSource audioSource;

	public bool isFilled;
	public bool isCoke;
	public bool isCapped;
	public bool isCrushed;

	void Awake () {
		animator = this.gameObject.GetComponent<Animator>();
		audioSource = this.gameObject.GetComponent<AudioSource>();
	}

	// Use this for initialization
	void Start () {
		canCapRend = GetComponent<Renderer>();
        //rend.materials[1] = null;
		toPoison = false;
		toCoke = false;
		coroutineCalled = false;
		isCapped = false;
		isCrushed = false;
		isFilled = false;
	}
	
	// Update is called once per frame
	void Update () {
		/*if (Input.GetKeyDown(KeyCode.Space)) {
			if (isCoke) {
				animator.SetTrigger("Pop");
				toPoison = true;
			}
			else {//if (rend.sharedMaterials[1] == material2) {
				animator.SetTrigger("Pop");
				toCoke = true;
			}
		}

		if (toPoison) {
			LerpMaterial1to2();
		}
		else if (toCoke) {
			LerpMaterial2to1();
		}*/
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.tag == "CokeLiquid") {
			if (!isFilled) {
				isFilled = true;
				this.gameObject.tag = "Coke";
				Debug.Log(this.gameObject.tag);
				animator.SetTrigger("Pop");
				liquidCoke.SetActive(true);
				liquidCoke.GetComponent<Animator>().SetTrigger("Rise");
				LerpMaterial2to1();
				audioSource.PlayOneShot(liquidDrop);
				GameObject.Destroy(other.gameObject);
			} else {
				GameObject.Destroy(other.gameObject);
			}
		}
		else if (other.gameObject.tag == "PoisonLiquid") {
			if (!isFilled) {
				isFilled = true;
				this.gameObject.tag = "Poison";
				Debug.Log(this.gameObject.tag);
				animator.SetTrigger("Pop");
				liquidPoison.SetActive(true);
				liquidPoison.GetComponent<Animator>().SetTrigger("Rise");
				LerpMaterial1to2();
				audioSource.PlayOneShot(liquidDrop);
				GameObject.Destroy(other.gameObject);
			} else {
				GameObject.Destroy(other.gameObject);
			}
		}
	}
	
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "conveyer_belt") {
			audioSource.PlayOneShot(canDrop);
		}
	}

	private void LerpMaterial1to2() {
		//float lerp = Mathf.PingPong(Time.time, duration) / duration;
		//rend.materials[1].Lerp(material1, material2, lerp);
		if (!coroutineCalled) {
			StartCoroutine(FixTo2());
		}
	}

	private void LerpMaterial2to1() {
		//float lerp = Mathf.PingPong(Time.time, duration) / duration;
		//rend.materials[1].Lerp(material2, material1, lerp);
		if (!coroutineCalled) {
			StartCoroutine(FixTo1());
		}
	}

	IEnumerator FixTo2 () {
		coroutineCalled = true;
		Material[] mat = canEmptyRend.materials;
		mat[1] = material2;
		canEmptyRend.materials = mat;
		yield return new WaitForSeconds(duration/2);
		coroutineCalled = false;
		toPoison = false;
		isCoke = false;
		//rend.materials[1] = material2;
	}
	
	IEnumerator FixTo1 () {
		coroutineCalled = true;
		Material[] mat = canEmptyRend.materials;
		mat[1] = material1;
		canEmptyRend.materials = mat;
		yield return new WaitForSeconds(duration/2);
		coroutineCalled = false;
		toCoke = false;
		isCoke = true;
		//rend.materials[1] = material1;
		Debug.Log(this.gameObject.tag);
	}

	public void CapCan() {
		audioSource.PlayOneShot(capClip);
		if (this.gameObject.tag == "Coke") {
			Material[] mat = canCapRend.materials;
			mat[1] = material1;	
			canCapRend.materials = mat;
			canCapRend.enabled = true;
			canEmptyRend.enabled = false;
			liquidCoke.SetActive(false);
		} else if (this.gameObject.tag == "Poison") {
			Material[] mat = canCapRend.materials;
			mat[1] = material2;	
			canCapRend.materials = mat;
			canCapRend.enabled = true;
			canEmptyRend.enabled = false;
			liquidPoison.SetActive(false);
		} else if (this.gameObject.tag == "Soda") {
			canCapRend.enabled = true;
			canEmptyRend.enabled = false;
		}
	}

	public void SplashLiquid() {
		StartCoroutine(SplashAndSmash());
	}

	IEnumerator SplashAndSmash() {
		yield return null;//new WaitForSeconds(0.1f);
		audioSource.PlayOneShot(crushClip);
		if (this.gameObject.tag == "Coke") {
			coke_splash.Play();
			Material[] mat = smashedRend.materials;
			mat[1] = material1;	
			smashedRend.materials = mat;
			smashedRend.enabled = true;
			canCapRend.enabled = false;
			//liquidCoke.SetActive(false);
		} else if (this.gameObject.tag == "Poison") {
			poison_splash.Play();
			Material[] mat = smashedRend.materials;
			mat[1] = material2;	
			smashedRend.materials = mat;
			smashedRend.enabled = true;
			canCapRend.enabled = false;
		} else if (this.gameObject.tag == "Soda") {
			smashedRend.enabled = true;
			canCapRend.enabled = false;
		}
	}
}
