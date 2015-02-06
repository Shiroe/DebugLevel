using UnityEngine;
using System.Collections;

public class preLoad: MonoBehaviour {

	public string toLoad;

	// handles the event when you step on the platform that is supposed to load next level
	void OnTriggerEnter(Collider x){

		if( x.gameObject.tag == "Player"){
			Game.Control.MoveSpawn(TP_controller.Instance.transform.position);
				
			if(!GameObject.Find(toLoad))
				StartCoroutine(loading());
			else
				ProgressTrack.checkUnload(toLoad, this);

		}

	}

	//ASYNC operation to load the next scene
	IEnumerator loading(){
		AsyncOperation async = Application.LoadLevelAdditiveAsync(toLoad);
		
		yield return async;
		
		if(async.isDone)
			ProgressTrack.checkUnload(toLoad, this);

	}

	//method to unload/ delete a scene object
	public void UnloadScene(string scene){
		Destroy(GameObject.Find(scene));
		
	}


	
}
