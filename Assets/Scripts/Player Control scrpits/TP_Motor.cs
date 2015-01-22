using UnityEngine;
using System.Collections;

public class TP_Motor : MonoBehaviour {

 	public static TP_Motor Instance;

	public float ForwardSpeed = 10f;
	public float BackwardSpeed = 5f;
	public float StrafeSpeed = 7f;
	public float slideSpeed = 12f;
	public float Gravity = 21;
	public float TerminalVelocity = 20f;
	public float JumpSpeed = 6f;
	public float SlideThreshold = 0.6f;
	public float MaxControllableSlideMagnitude = 0.4f;

	private Vector3 slideDirection;

	public Vector3 MoveVector { get; set; }
	public float VerticalVelocity {get; set;}

	void Awake () {
		Instance = this;
	}

	//inefficient to use the normal Update, no need to call at every frame

	public void UpdateMotor(){
		// call snap  align
		SnapAlignCharacterWithCamera();
		//call the ProcessMotion
		ProcessMotion();
	}

	void SnapAlignCharacterWithCamera(){
		//are we moving?
		//if so rotate te character to match the direction the camera is facing 
		if (MoveVector.x != 0 || MoveVector.z != 0) {
			transform.rotation = Quaternion.Euler(transform.eulerAngles.x,
			                                      Camera.main.transform.eulerAngles.y,
			                                      transform.eulerAngles.z);
		}
	}

	void ProcessMotion(){
		//call on transform direction
		MoveVector = transform.TransformDirection(MoveVector);
		//check the magnitude of the vector that comes back, if not normalize
		if (MoveVector.magnitude > 1)
			MoveVector = Vector3.Normalize(MoveVector);
		//apply sliding if its the case
		ApplySlide();
		//scale this by movespeed 
		MoveVector *= MoveSpeed();
		//reapply vertical velocity
		MoveVector = new Vector3(MoveVector.x, VerticalVelocity,
		                         MoveVector.z);
		//apply gravity
		ApplyGravity();
		//then move!
		TP_controller.CharacterController.Move(MoveVector * Time.deltaTime);
	}

	void ApplyGravity(){
		if(MoveVector.y > - TerminalVelocity)
			MoveVector = new Vector3(MoveVector.x, MoveVector.y - Gravity * Time.deltaTime,
			                         MoveVector.z);

		if(TP_controller.CharacterController.isGrounded && MoveVector.y < -1)
			MoveVector = new Vector3(MoveVector.x, -1,
			                         MoveVector.z);
	}

	float MoveSpeed(){
		var moveSpeed = 0f;

		switch(TP_Animator.Instance.MoveDirection){
		case TP_Animator.Direction.Stationary:
				moveSpeed = 0;
			break;
		case TP_Animator.Direction.Forward:
			moveSpeed = ForwardSpeed;
			break;

		case TP_Animator.Direction.Backward:
			moveSpeed = BackwardSpeed;
			break;

		case TP_Animator.Direction.Left:
			moveSpeed = StrafeSpeed;
			break;

		case TP_Animator.Direction.Right:
			moveSpeed = StrafeSpeed;
			break;

		case TP_Animator.Direction.LeftBackward:
			moveSpeed = BackwardSpeed;
			break;

		case TP_Animator.Direction.LeftForward:
			moveSpeed = BackwardSpeed;
			break;

		case TP_Animator.Direction.RightBackward:
			moveSpeed = ForwardSpeed;
			break;

		case TP_Animator.Direction.RightForward:
			moveSpeed = ForwardSpeed;
			break;

		}

		if(slideDirection.magnitude > 0)
			moveSpeed = slideSpeed;

		return moveSpeed;
	}

	void ApplySlide(){
		if(!TP_controller.CharacterController.isGrounded)
			return;

		slideDirection = Vector3.zero;
		RaycastHit hitInfo;

		if(Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hitInfo)){

			if(hitInfo.normal.y < SlideThreshold)
				slideDirection = new Vector3(hitInfo.normal.x, -hitInfo.normal.y, hitInfo.normal.z);
		}
		if(slideDirection.magnitude < MaxControllableSlideMagnitude)
			MoveVector += slideDirection;
		else
			MoveVector = slideDirection;
	}

	public void Jump(){
		if(TP_controller.CharacterController.isGrounded)
			VerticalVelocity = JumpSpeed;
	}

}
