﻿using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game Control;
	Vector3 spawnLoc;
	bool Menu;
	string CurrentLevel;

	// Use this for initialization
	void Awake () {
		if(Control == null){
			DontDestroyOnLoad(gameObject);
			Control = this;
		}
		else if(Control != this)
			Destroy(gameObject);
			spawnLoc = isSpawned.Instance.transform.position;
		}

	public void setSpawn(Vector3 loc){
		spawnLoc = loc;
	}

	public void MoveSpawn(Vector3 loc){
		isSpawned.Instance.transform.position = loc;
	}

	void CaptureResumeData(){
		spawnLoc = isSpawned.Instance.getLoc();
		CurrentLevel = ProgressTrack.currentScene;
	}

	void Update(){
		if(Input.GetButtonDown("Cancel")){

			if(Time.timeScale == 1){
				CaptureResumeData();
				TP_Camera.Instance.enabled = false;
				Time.timeScale = 0;
			}
			else{
				Time.timeScale = 1;
				TP_Camera.Instance.enabled = true;
			}
		}
	}

}
