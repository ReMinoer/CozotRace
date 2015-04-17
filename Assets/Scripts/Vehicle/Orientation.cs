using UnityEngine;
using System.Collections;

public class Orientation : MonoBehaviour {

	// Use this for initialization
	
	// Update is called once per frame
	public void Update() {
		Ray rayFR = new Ray (transform.position, -transform.up);
		Ray rayFL = new Ray (transform.position, -transform.up);
		Ray rayBR = new Ray (transform.position, -transform.up);
		Ray rayBL = new Ray (transform.position, -transform.up);
		
		Vector3 upDir;
		Transform frontRightTransform = null;
		Transform frontLeftTransform = null;
		Transform backRightTransform = null;
		Transform backLeftTransform = null;
		Transform childTransform = transform.FindChild ("car02");
		if (childTransform != null) {
			frontRightTransform = childTransform.gameObject.transform.FindChild ("FrontRight");
			frontLeftTransform = childTransform.gameObject.transform.FindChild ("FrontLeft");
			backRightTransform = childTransform.gameObject.transform.FindChild ("BackRight");
			backLeftTransform = childTransform.gameObject.transform.FindChild ("BackLeft");
		}
		if (frontRightTransform != null)
			rayFR = new Ray (frontRightTransform.position, -frontRightTransform.up);
		if (frontLeftTransform != null)
			rayFL = new Ray (frontLeftTransform.position, -frontLeftTransform.up);
		if (backRightTransform != null)
			rayBR = new Ray (backRightTransform.position, -backRightTransform.up);
		if (backLeftTransform != null)
			rayBL = new Ray (backLeftTransform.position, -backLeftTransform.up);
		Vector3 face = transform.forward;
		RaycastHit hitFR, hitFL, hitBR, hitBL;
		Physics.Raycast (rayFR, out hitFR);
		Physics.Raycast (rayFL, out hitFL);
		Physics.Raycast (rayBR, out hitBR);
		Physics.Raycast (rayBL, out hitBL);
		upDir = (Vector3.Cross (hitBR.point - Vector3.up, hitBL.point - Vector3.up) +
		         Vector3.Cross (hitBL.point - Vector3.up, hitFL.point - Vector3.up) +
		         Vector3.Cross (hitFL.point - Vector3.up, hitFR.point - Vector3.up) +
		         Vector3.Cross (hitFR.point - Vector3.up, hitBR.point - Vector3.up)
		         ).normalized;
		//transform.LookAt (face, upDir);
	}
}
