using UnityEngine;

public class Orientation : MonoBehaviour
{
    // Use this for initialization

    // Update is called once per frame
    public void Update()
    {
        var rayFr = new Ray(transform.position, -transform.up);
        var rayFl = new Ray(transform.position, -transform.up);
        var rayBr = new Ray(transform.position, -transform.up);
        var rayBl = new Ray(transform.position, -transform.up);

        Vector3 upDir;
        Transform frontRightTransform = null;
        Transform frontLeftTransform = null;
        Transform backRightTransform = null;
        Transform backLeftTransform = null;
        Transform childTransform = transform.FindChild("car02");
        if (childTransform != null)
        {
            frontRightTransform = childTransform.gameObject.transform.FindChild("FrontRight");
            frontLeftTransform = childTransform.gameObject.transform.FindChild("FrontLeft");
            backRightTransform = childTransform.gameObject.transform.FindChild("BackRight");
            backLeftTransform = childTransform.gameObject.transform.FindChild("BackLeft");
        }
        if (frontRightTransform != null)
            rayFr = new Ray(frontRightTransform.position, -frontRightTransform.up);
        if (frontLeftTransform != null)
            rayFl = new Ray(frontLeftTransform.position, -frontLeftTransform.up);
        if (backRightTransform != null)
            rayBr = new Ray(backRightTransform.position, -backRightTransform.up);
        if (backLeftTransform != null)
            rayBl = new Ray(backLeftTransform.position, -backLeftTransform.up);
        Vector3 face = transform.forward;
        RaycastHit hitFr, hitFl, hitBr, hitBl;
        Physics.Raycast(rayFr, out hitFr);
        Physics.Raycast(rayFl, out hitFl);
        Physics.Raycast(rayBr, out hitBr);
        Physics.Raycast(rayBl, out hitBl);
        upDir = (Vector3.Cross(hitBr.point - Vector3.up, hitBl.point - Vector3.up) +
                 Vector3.Cross(hitBl.point - Vector3.up, hitFl.point - Vector3.up) +
                 Vector3.Cross(hitFl.point - Vector3.up, hitFr.point - Vector3.up) +
                 Vector3.Cross(hitFr.point - Vector3.up, hitBr.point - Vector3.up)
            ).normalized;
        //transform.LookAt (face, upDir);
    }
}