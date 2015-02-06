using UnityEngine;
using System.Collections;

public class isSpawned : MonoBehaviour {
	public GameObject player;
	public GameObject GameController;
	GameObject monster;
	public  static isSpawned Instance;

	//check if we have a player around, if not spawn him
	void Awake(){
		if(Instance == null)
			Instance = this;
		else if(Instance != this)
			Destroy(gameObject);

		if(!GameObject.FindGameObjectWithTag("Player")){
			monster = (GameObject)Instantiate(player, transform.position, Quaternion.identity);
			ProgressTrack.currentScene = transform.parent.name;
		}

		if(!GameObject.FindGameObjectWithTag("Ambient")){
			GameObject Moon = (GameObject)Instantiate(Resources.Load("Moon"));
		}

		if(!GameObject.FindGameObjectWithTag("Controller")){
			GameController = (GameObject)Instantiate(GameController, Vector3.zero, Quaternion.identity);
		}

	}

	public Vector3 getLoc(){
		return monster.transform.position;
	}

}
