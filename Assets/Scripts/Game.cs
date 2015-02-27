using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public static Game Control;
	Vector3 spawnLoc;
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

	}
}
