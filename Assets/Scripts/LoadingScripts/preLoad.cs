using UnityEngine;
using System.Collections;

public class preLoad: MonoBehaviour {

	public string toLoad;
	bool isLoaded = false;
	public Vector3 coords;

	void OnTriggerEnter(Collider x){

		if(!isLoaded && x.gameObject.tag == "Player")
			StartCoroutine(loading());
	}

	IEnumerator loading(){
		Debug.Log("xekinhse!");
		AsyncOperation async = Application.LoadLevelAdditiveAsync(toLoad);
		
		yield return async;
		
		if(async.isDone){
			isLoaded = true;
			GameObject scene = new GameObject();

			scene = GameObject.Find(toLoad);
			scene.transform.position = coords;

		}
	}
}
