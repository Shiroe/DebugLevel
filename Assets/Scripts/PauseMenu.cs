using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {

	private bool _showFirstMenu = false;
	private bool _showOptionsMenu = false;
	private bool _showAudioOptions = false;
	private bool _showGraphicOptions = false;
	private int buttonWidth = 200;
	private int buttonHeight = 50;
	private int groupWidth = 200;
	private int groupHeight = 230;
	private float masterVolume = 0.4f;

	
	// Use this for initialization
	void Start () {
		masterVolume = PlayerPrefs.GetFloat ("Master Volume", masterVolume);
		if(PlayerPrefs.HasKey ("Master Volume")) {
			AudioListener.volume = PlayerPrefs.GetFloat ("Master Volume", masterVolume);
		}
		else{
			PlayerPrefs.SetFloat ("Master Volume", masterVolume);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp(KeyCode.Escape)){
			//TogglePause();
			WhichMenuToShow();
		}
	}

	void WhichMenuToShow(){
		if(_showFirstMenu == false && _showOptionsMenu == false && _showAudioOptions == false && _showGraphicOptions == false){
			TogglePause();
			_showFirstMenu = true;
		}
		else if(_showFirstMenu == true && _showOptionsMenu == false && _showAudioOptions == false && _showGraphicOptions == false){
			TogglePause();
			_showFirstMenu = false;
		}
		else if(_showFirstMenu == false && _showOptionsMenu == true && _showAudioOptions == false && _showGraphicOptions == false){
			_showOptionsMenu = false;
			_showFirstMenu = true;
		}
		else if(_showFirstMenu == false && _showOptionsMenu == false && _showAudioOptions == true && _showGraphicOptions == false){
			_showAudioOptions = false;
			_showOptionsMenu = true;
		}
		else if(_showFirstMenu == false && _showOptionsMenu == false && _showAudioOptions == false && _showGraphicOptions == true){
			_showGraphicOptions = false;
			_showOptionsMenu = true;
		}
		else{
			_showFirstMenu = false;
			_showOptionsMenu = false;
			_showAudioOptions = false;
			_showGraphicOptions = false;
		}
	}

	void TogglePause(){
		if(Time.timeScale == 1){
			TP_Camera.Instance.enabled = false;
			Time.timeScale = 0;
		}
		else{
			Time.timeScale = 1;
			TP_Camera.Instance.enabled = true;
		}
	}

	void OnGUI(){
		FirstMenu();
		OptionsMenu();
		AudioOptions();
		GraphicOptions();
	}

	void FirstMenu(){
		
		if (_showFirstMenu) {
			GUI.BeginGroup(new Rect(((Screen.width/2) - (groupWidth/2)),((Screen.height/2) - (groupHeight/2)), groupWidth, groupHeight));
			
			if(GUI.Button (new Rect(0,0, buttonWidth, buttonHeight), "Resume Game")){
				TogglePause();
				_showFirstMenu = false;
			}
			if(GUI.Button (new Rect(0,60, buttonWidth, buttonHeight), "Restart Game")){
				TogglePause();
				Application.LoadLevel(0);
			}
			if(GUI.Button (new Rect(0,120, buttonWidth, buttonHeight), "Options")){
				_showFirstMenu = false;
				_showOptionsMenu = true;//Option menu set to true;
			}
			if(GUI.Button (new Rect(0,180, buttonWidth, buttonHeight), "Quit Game")){
				Application.Quit(); //Works only with Build & Run, not in PIE mode.
			}
			GUI.EndGroup();
		}
	}

	void AudioOptions(){
		if(_showAudioOptions){
			GUI.BeginGroup (new Rect (((Screen.width / 2) - (groupWidth / 2)) - 100, ((Screen.height / 2) - (groupHeight / 2)), 410, 100));
		
			GUI.Box (new Rect (0, 0, groupWidth * 2, groupHeight / 2), "Audio Settings");
		
			GUI.Label (new Rect (10, 30, 100, 30), "Master Volume");
			masterVolume = GUI.HorizontalSlider (new Rect (120, 35, 200, 30), masterVolume, 0.0f, 1.0f);
			AudioListener.volume = masterVolume;

			GUI.EndGroup ();

			if(GUI.Button (new Rect(((Screen.width/2) - (groupWidth/2))-105,((Screen.height/2) +120 - (groupHeight/2)), buttonWidth, buttonHeight),"Apply")){
				PlayerPrefs.SetFloat("Master Volume", masterVolume);
			}

			if(GUI.Button (new Rect(((Screen.width/2) - (groupWidth/2))+105,((Screen.height/2) +120 - (groupHeight/2)), buttonWidth, buttonHeight), "Back")){
				_showAudioOptions = false;
				_showOptionsMenu = true;
			}
		}
	}

	void GraphicOptions(){
		if (_showGraphicOptions) {
				//TO DO
			if(GUI.Button (new Rect(((Screen.width/2) - (groupWidth/2)),((Screen.height/2) +120 - (groupHeight/2)), buttonWidth, buttonHeight), "Back")){
				_showGraphicOptions = false;
				_showOptionsMenu = true;
			}
		}
	}

	void OptionsMenu(){
		if(_showOptionsMenu){
			GUI.BeginGroup(new Rect(((Screen.width/2) - (groupWidth/2)),((Screen.height/2) - (groupHeight/2)), groupWidth, groupHeight));

			if(GUI.Button (new Rect(0,0, buttonWidth, buttonHeight), "Audio Settings")){
				_showOptionsMenu = false;
				_showAudioOptions = true;
			}
			if(GUI.Button (new Rect(0,60, buttonWidth, buttonHeight), "Graphic Settings")){
				_showOptionsMenu = false;
				_showGraphicOptions = true;
			}
			if(GUI.Button (new Rect(0,120, buttonWidth, buttonHeight), "Back")){
				_showFirstMenu = true;
				_showOptionsMenu = false;
			}
			GUI.EndGroup();
		}
	}
}
