using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour {

	public GameObject Menu;
	public GameObject Instructions;
	public GameObject Scores;

	public TextMeshProUGUI highScore;
	public TextMeshProUGUI lastScore;

	void Start() {
		Menu.SetActive(true);
		highScore.text = "HIGH SCORE: " + PlayerPrefs.GetInt("HIGHSCORE", 0);
		lastScore.text = "LAST SCORE: " + PlayerPrefs.GetInt("LASTSCORE", 0);
	}

	public void StartGame() {
		SceneManager.LoadScene(1);
	}

	public void InstructionsPage() {
		Menu.SetActive(false);
		Instructions.SetActive(true);
	} 

	public void ScoresPage() {
		Menu.SetActive(false);
		Scores.SetActive(true);
	} 

	public void QuitGame() {
		Application.Quit();
	}

	public void GoBack() {
		Instructions.SetActive(false);
		Menu.SetActive(true);
	}
}
