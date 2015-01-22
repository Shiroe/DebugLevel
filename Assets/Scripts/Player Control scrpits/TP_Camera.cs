using UnityEngine;
using System.Collections;

public class TP_Camera : MonoBehaviour {

	public static TP_Camera Instance;

	public Transform TargetLookAt;
	public float Distance = 5;
	public float DistanceMin = 2;
	public float DistanceMax = 10;
	public float X_mouseSensitivity = 8;
	public float Y_mouseSensitivity = 8;
	public float MouseWheelSensitivity = 20;
	public float X_Smooth =  0.08f;
	public float Y_Smooth =  0.06f;
	public float Y_MinLimit = -40;
	public float Y_MaxLimit = 80;
	public float DistanceSmooth = 0.4f;
	public float DistanceResumeSmooth = 1f;
	public float OcclusionDistanceStep = 0.5f;
	public int MaxOcclusionChecks = 10;

	private float mouseX = 0;
	private float mouseY = 0;
	private float velX = 0;
	private float velY = 0;
	private float velZ = 0;
	private float startDistance = 5;
	private Vector3 position = Vector3.zero;
	private float desiredDistance = 0;
	private float velDistance = 0;
	private Vector3 desiredPosition = Vector3.zero;
	private float distanceSmooth = 0f;
	private float preOccludedDistance = 0;

	// Use this for initialization
	void Awake () {
		Instance = this;
	}

	void Start(){
		Distance = Mathf.Clamp(Distance,DistanceMin, DistanceMax);
		startDistance = Distance;
		Reset ();
	}

	public void Reset(){
		mouseX = 0;
		mouseY = 10;
		Distance = startDistance;
		desiredDistance = Distance;
		preOccludedDistance = Distance;

	}

	//we want all camera calculations to happen last
	void LateUpdate () {
		if(TargetLookAt == null)
			return;

		HandlePlayerInput();

		CalculateDesiredPosition();
		OcclusionHandler ();

		UpdatePosition();
	}

	void OcclusionHandler(){
		Vector3 occRay = desiredPosition - TargetLookAt.position;
		float thinRadius  = 0.15f;
		float thickRadius = 0.3f;

		var colPoint = Helper.GetCollisionSimple(TargetLookAt.position, desiredPosition, thinRadius, true);
		var colPointThick = Helper.GetCollisionSimple(TargetLookAt.position, desiredPosition, thickRadius, false);

		var colPointThickProjectedOnRay = Vector3.Project(colPointThick - TargetLookAt.position, occRay.normalized) +TargetLookAt.position;
		var vecToProjected = (colPointThickProjectedOnRay - colPointThick).normalized;
		var colPointThickProjectedOnThisCapsule = colPointThickProjectedOnRay - vecToProjected * thinRadius;
		var thin2ThickDist = Vector3.Distance(colPointThickProjectedOnThisCapsule, colPointThick);
		var thin2ThickDistNorm = thin2ThickDist / (thickRadius - thinRadius);
		Distance = desiredDistance;

		float currentColDist = Vector3.Distance(TargetLookAt.position, colPoint);
		float currentColDistThick = Vector3.Distance(TargetLookAt.position, colPointThickProjectedOnRay);
		currentColDist = Mathf.Lerp(currentColDistThick, currentColDist, thin2ThickDistNorm);

		Distance = currentColDist < Distance? currentColDist : Mathf.SmoothStep(Distance, currentColDist, DistanceResumeSmooth);
		Vector3 vec = desiredPosition - TargetLookAt.position;

		desiredPosition = TargetLookAt.position + vec.normalized * Distance;
	}

	void HandlePlayerInput(){
		var deadZone = 0.01f;
		//debug check, so you can freely move your mouse
		if(Input.GetMouseButton(1)){
			mouseX += Input.GetAxis("Mouse X") * X_mouseSensitivity;
			mouseY -= Input.GetAxis("Mouse Y") * Y_mouseSensitivity;
		}

		mouseY = Helper.ClampAngle(mouseY, Y_MinLimit, Y_MaxLimit);

		if(Input.GetAxis("Mouse ScrollWheel") < - deadZone || Input.GetAxis("Mouse ScrollWheel") >  deadZone){
			desiredDistance = Mathf.Clamp(Distance - Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity,
			                              DistanceMin, DistanceMax);
			preOccludedDistance = desiredDistance;
			distanceSmooth = DistanceSmooth;
		}
	}

	void CalculateDesiredPosition(){

		//ResetDesiredDistance();
		//Evaluate Distance
		Distance = Mathf.SmoothDamp(Distance, desiredDistance, ref velDistance, distanceSmooth);

		//Calculate desired position
		desiredPosition = CalculatePosition(mouseY, mouseX, Distance);
	}

	Vector3 CalculatePosition(float rotationX, float rotationY, float distance){
		Vector3 direction = new Vector3(0, 0, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);

		return TargetLookAt.position + rotation * direction;
	}


	void UpdatePosition(){
		var posX = Mathf.SmoothDamp(position.x, desiredPosition.x, ref velX, X_Smooth);
		var posY = Mathf.SmoothDamp(position.y, desiredPosition.y, ref velY, Y_Smooth);
		var posZ = Mathf.SmoothDamp(position.z, desiredPosition.z, ref velZ, X_Smooth);

		position = new Vector3(posX, posY, posZ);
		transform.position = position;

		transform.LookAt(TargetLookAt);
	}

	public static void UseExistingOrCreateNewMainCam(){

		GameObject tempCamera;
		GameObject targetLookAt;
		TP_Camera myCamera;

		if(Camera.main != null){
			tempCamera = Camera.main.gameObject;
		}
		else{
			tempCamera = new GameObject("Main Camera");
			tempCamera.AddComponent("Camera");
			tempCamera.tag = "MainCamera";
		}

		if(tempCamera.GetComponent("TP_Camera") == null)
			tempCamera.AddComponent("TP_Camera");
		myCamera = tempCamera.GetComponent("TP_Camera") as TP_Camera;

		targetLookAt = GameObject.Find("targetLookAt") as GameObject;

		if(targetLookAt == null){
			targetLookAt =  new GameObject("targetLookAt");
			targetLookAt.transform.position = Vector3.zero;
		}

		myCamera.camera.farClipPlane = 140;
		myCamera.TargetLookAt = targetLookAt.transform;
	}
}
