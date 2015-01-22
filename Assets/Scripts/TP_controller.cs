using UnityEngine;
using System.Collections;

public class TP_controller : MonoBehaviour {

	public static CharacterController CharacterController;
	public static TP_controller Instance;
	public string CurrentLevel = "gameStart";

	// Use this for initialization
	void Awake(){

		// assign references into our static fields 
		CharacterController = GetComponent ("CharacterController") as CharacterController;
		Instance = this;
		// performs initial camera check  - exists?
		TP_Camera.UseExistingOrCreateNewMainCam();


	}

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		//verify main camera existance 
		if (Camera.main == null)
			return;
		//call getLocomotionInput()
		getLocomotionInput();

		HandleActionInput();
		//tell TP_motor to update itself 
		TP_Motor.Instance.UpdateMotor ();
	}

	void getLocomotionInput(){
		//create variable to hold dead-space (depends on analogue input)
		var deadZone = 0.1f;
		//do gravity and set vertical velocity in TP_motor
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		//zeroing out the move 
		TP_Motor.Instance.MoveVector = Vector3.zero;
		//check that vertical axis is not in deadspace
		if (Input.GetAxis ("Vertical") > deadZone || Input.GetAxis ("Vertical") < -deadZone)
			TP_Motor.Instance.MoveVector += new Vector3 (0, 0, Input.GetAxis ("Vertical"));
		//same for the X - axis
		if (Input.GetAxis ("Horizontal") > deadZone || Input.GetAxis ("Horizontal") < -deadZone)
			TP_Motor.Instance.MoveVector += new Vector3 (Input.GetAxis ("Horizontal"), 0, 0);

		TP_Animator.Instance.DetermineCurrentMoveDirection();
	}

	void HandleActionInput(){
		if(Input.GetButton("Jump")){
			Jump();	
		}
	}

	void Jump(){
		TP_Motor.Instance.Jump();
	}

}
