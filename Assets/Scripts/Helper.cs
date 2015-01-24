using UnityEngine;
//using System.Collections;

public static class Helper {

	public struct ClipPlanePoints{
		public Vector3 UpperLeft;
		public Vector3 UpperRight;
		public Vector3 LowerLeft;
		public Vector3 LowerRight;
	}

	public static float ClampAngle(float angle, float min, float max){
		do{
			if(angle < -360)
				angle += 360;
			if(angle > 360)
				angle -= 360;
		}
		while( angle < -360 || angle > 360);

		return Mathf.Clamp(angle, min, max);
	}

	public static ClipPlanePoints ClipPlaneAtNear(Vector3 pos){
		var clipPlanePoints = new ClipPlanePoints();

		if(Camera.main == null)
			return clipPlanePoints;

		var transform = Camera.main.transform;
		var halfFOV = (Camera.main.fieldOfView /2) * Mathf.Deg2Rad;
		var aspect = Camera.main.aspect;
		var distance = Camera.main.nearClipPlane;
		var height = distance * Mathf.Tan(halfFOV);
		var width = height * aspect;

		clipPlanePoints.LowerRight = pos + transform.right * width;
		clipPlanePoints.LowerRight -= transform.up * height;
		clipPlanePoints.LowerRight += transform.forward * distance;

		clipPlanePoints.LowerLeft = pos - transform.right * width;
		clipPlanePoints.LowerLeft -= transform.up * height;
		clipPlanePoints.LowerLeft += transform.forward * distance;

		clipPlanePoints.UpperRight = pos + transform.right * width;
		clipPlanePoints.UpperRight += transform.up * height;
		clipPlanePoints.UpperRight += transform.forward * distance;

		clipPlanePoints.UpperLeft = pos - transform.right * width;
		clipPlanePoints.UpperLeft += transform.up * height;
		clipPlanePoints.UpperLeft += transform.forward * distance;

		return clipPlanePoints;
	}

	public static Vector3 GetCollisionSimple(Vector3 target,Vector3 cameraOptPos, float radius, bool pushByNormal){
		//might need tweeking 
		float farEnough = 0.5f;
	
		RaycastHit occHit;
		Vector3 origin = target;
		Vector3 occRay = origin - cameraOptPos;
		float dt = Vector3.Dot(cameraOptPos, occRay);

		if (dt < 0) 
			occRay *= -1;
	
		if (Physics.SphereCast(origin, radius, occRay.normalized, out occHit, farEnough))
			origin = origin + occRay.normalized * occHit.distance;
		else
			origin += occRay.normalized * farEnough;
	
		occRay = origin - cameraOptPos;

		if (Physics.SphereCast(origin, radius, -occRay.normalized, out occHit, occRay.magnitude))
			return pushByNormal? occHit.point + occHit.normal*radius : occHit.point;
		else
			return cameraOptPos;
	}

	public static int Search(string name, string[] table, int length){
		var count = -1;

		for(int i = 0; i < length; i++){
			if(table[i].Equals(name)){
				count = i;
				break;
			}
		}

		return count;
	}
}
