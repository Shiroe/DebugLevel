using UnityEngine;
using System.Collections;

public class isSpawned : MonoBehaviour {
	public GameObject player;
	GameObject monster;
	// Use this for initialization
	void Start(){

		if(!GameObject.FindGameObjectWithTag("Player")){
			monster = (GameObject)Instantiate(player, transform.position, Quaternion.identity);

		}

	}

	void Update(){
		transform.position = monster.transform.position;
		if(!GameObject.FindGameObjectWithTag("Ambient")){
		   GameObject Moon = (GameObject)Instantiate(Resources.Load("Moon"));
		}
	}
}
