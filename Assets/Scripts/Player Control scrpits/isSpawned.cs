using UnityEngine;
using System.Collections;

public class isSpawned : MonoBehaviour {
	public GameObject player;
	GameObject monster;
	// Use this for initialization
	void Awake(){

		if(!GameObject.FindGameObjectWithTag("Player")){
			monster = (GameObject)Instantiate(player, transform.position, Quaternion.identity);
			ProgressTrack.currentScene = transform.parent.name;
		}
		if(!GameObject.FindGameObjectWithTag("Ambient")){
			GameObject Moon = (GameObject)Instantiate(Resources.Load("Moon"));
		}

	}

	void Update(){
		transform.position = monster.transform.position;


	}
}
