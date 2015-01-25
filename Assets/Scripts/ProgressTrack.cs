using UnityEngine;
using System.Collections;

public static class ProgressTrack  {

	public static string currentScene;
	public static string previousScene;

	public static void checkUnload(string toLoad, preLoad x){
		//Debug.Log(toLoad);
		if(!toLoad.Equals(currentScene) && !toLoad.Equals(previousScene) ){
			if(!currentScene.Equals(x.transform.parent.name)){
				x.UnloadScene(currentScene);
				currentScene = toLoad;
			}
			else{
				x.UnloadScene(previousScene);
				SwapScenes(toLoad);
			}
		}
		else if(x.transform.parent.name.Equals(previousScene)){
			//Debug.Log(x.transform.parent.name);
			//if(!x.transform.parent.name.Equals(currentScene))
			SwapScenes(x.transform.parent.name);
		}

	}

	public static void SwapScenes(string toLoad){
			previousScene = currentScene;
			currentScene = toLoad;
	}


}
