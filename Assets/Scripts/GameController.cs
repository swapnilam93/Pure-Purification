using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController Singleton;
	public bool isMusicMode = false;

	public GameObject gameOverUI;
	public CokeCollector cokeCollector;
	public CokeGenerator cokeGenerator;

	void Awake(){
		Singleton = this;
		cokeCollector = GameObject.Find("Collector").GetComponent<CokeCollector>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void GameOver() {
		cokeGenerator.generate = false;
		StartCoroutine(FinishGame());
	}

	IEnumerator FinishGame() {
		yield return new WaitForSeconds(5f);
		gameOverUI.SetActive(true);
		PlayerPrefs.SetInt("LASTSCORE", cokeCollector.score);
		if (cokeCollector.score > PlayerPrefs.GetInt("HIGHSCORE")) {
			PlayerPrefs.SetInt("HIGHSCORE", cokeCollector.score);
			gameOverUI.GetComponent<Text>().text = "NEW HIGHSCORE: " + cokeCollector.score;
		} else {
			gameOverUI.GetComponent<Text>().text = "SCORE: " + cokeCollector.score;
		}
		PlayerPrefs.Save();
	}

	public void GoBack() {
		SceneManager.LoadScene(0);
	}
}
