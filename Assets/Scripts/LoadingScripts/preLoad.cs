using UnityEngine;
using System.Collections;

public class preLoad: MonoBehaviour {

	public string toLoad;
	//bool isLoaded = false;
	//public Vector3 coords;

	void OnTriggerEnter(Collider x){

		if( x.gameObject.tag == "Player"){
			if(!GameObject.Find(toLoad))
				StartCoroutine(loading());
			else
				ProgressTrack.checkUnload(toLoad, this);
		}

	}

	IEnumerator loading(){
		AsyncOperation async = Application.LoadLevelAdditiveAsync(toLoad);
		
		yield return async;
		
		if(async.isDone){
			ProgressTrack.checkUnload(toLoad, this);
			//isLoaded = true;
			//GameObject scene = new GameObject();

			//scene = GameObject.Find(toLoad);


			//scene.transform.position = coords;

		}
	}

	public void UnloadScene(string scene){
		Destroy(GameObject.Find(scene));
		
	}


	
}
