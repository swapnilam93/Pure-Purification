/*
	Based On:
	https://www.gamasutra.com/blogs/YuChao/20170316/293814/Music_Syncing_in_Rhythm_Games.php
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;

public class BeatController : MonoBehaviour {
	[SerializeField]
	CokeGenerator cokeGenerator;
	[System.Serializable]
	public class MusicClip {
		public AudioClip clip;
		public float offset;
		public float bpm;
		public float beatsShownInAdvance = 4, stop2later = 2;
		public bool isRecording;
		public List<float> beats;
		public List<bool> isHardBeat;
		public TextAsset BeatFile;
	}
	[SerializeField]
	List<MusicClip> clips;
	[SerializeField]
	AudioSource audioSource;
	[SerializeField]
	Transform Stop1, Stop2;
	MusicClip currClip;
	int currClipIdx = 0;
	float clipLength;
	//the current position of the song (in seconds)
	float songPosition;
	//the current position of the song (in beats)
	float songPosInBeats;
	//the duration of a beat
	float secPerBeat;
	//how much time (in seconds) has passed since the song started
	float dsptimesong;
	int nextIndex;
	public bool canStartPlaying = false;
	public bool isPlaying = false;
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	// Update is called once per frame
	void Update () {
		if (canStartPlaying) {
			canStartPlaying = false;
			isPlaying = true;
			Debug.Log(currClipIdx);
			currClip = clips[currClipIdx++];
			clipLength = currClip.clip.length;
			if (currClip.isRecording) {
				currClip.beats = new List<float>();
				currClip.isHardBeat = new List<bool>();
			}
			// preload???
			//calculate how many seconds is one beat
			//we will see the declaration of bpm later
			secPerBeat = 60f / currClip.bpm;
			//record the time when the song starts
			dsptimesong = (float) AudioSettings.dspTime + currClip.offset;
			nextIndex = 0;
			audioSource.clip = currClip.clip;
			//start the song
			audioSource.Play();
			// while (!audioSource.isPlaying) {

			// }
		}
		if (isPlaying) {
			//calculate the position in seconds
			songPosition = (float) (AudioSettings.dspTime - dsptimesong);
			//calculate the position in beats
			songPosInBeats = songPosition / secPerBeat;
			if (!currClip.isRecording) {
				if (currClip.BeatFile != null) {
					var text = currClip.BeatFile.text.Split('\n');
					currClip.beats = text[0].Split(',').Select(x => float.Parse(x)).ToList();
					currClip.isHardBeat = text[1].Split(',').Select(x => bool.Parse(x)).ToList();
				}
				if (nextIndex < currClip.beats.Count && currClip.beats[nextIndex] < songPosInBeats + currClip.beatsShownInAdvance)
				{
					var coke = cokeGenerator.GenerateRemoteCoke();
					var can = coke.GetComponent<BeatControlledCan>();
					float nextBeat = currClip.beats[nextIndex];
					//initialize the fields of the music note
					can.Stop1 = Stop1;
					can.Stop2 = Stop2;
					can.dspTime1 = dsptimesong + nextBeat * secPerBeat;
					can.dspTime2 = dsptimesong + (nextBeat + currClip.stop2later) * secPerBeat;
					can.audioSource = audioSource;
					nextIndex++;
				}
			}
			else {
				bool hasNote = false, smash = false;
				if (Input.GetKeyDown(KeyCode.N)) {
					hasNote = true;
				}
				if (Input.GetKeyDown(KeyCode.M)) {
					hasNote = true;
					smash = true;
				}
				if (hasNote) {
					float tmpSongPos = Mathf.Round(songPosInBeats * 2) / 2;
					currClip.beats.Add(tmpSongPos - currClip.stop2later);
					currClip.isHardBeat.Add(smash);
					Debug.Log("Add smash: " + smash);
				}
				
			}
			
			if (!audioSource.isPlaying) {
				isPlaying = false;
				if (clips.Count > currClipIdx) {
					canStartPlaying = true;
				} else {
					//game over
					//StartCoroutine(GameOver());
				}
				if (currClip.isRecording) {
					string path = "Assets/Resources/" + currClip.clip.name + System.DateTime.Now.ToString("ddHHmmss") + ".txt";
					// File.CreateText(path);
					StreamWriter writer = new StreamWriter(path);
					writer.WriteLine(string.Join(",", currClip.beats));
					writer.WriteLine(string.Join(",", currClip.isHardBeat));
					writer.Close();
					Debug.Log(currClip.beats);
					Debug.Log(currClip.isHardBeat);

				}
			}
		}
	}

	IEnumerator GameOver() {
		yield return new WaitForSeconds(5f);
		GameObject.Find("GameManager").GetComponent<GameController>().GameOver();
	}
}
