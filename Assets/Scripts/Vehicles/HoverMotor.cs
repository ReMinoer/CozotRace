using UnityEngine;

public class HoverMotor : MonoBehaviour
{
    public float FloatingHeight = 1.5f;
    public float HoverForce = 65f;

    public void FixedUpdate()
    {
        var ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, FloatingHeight))
        {
            float proportionalHeight = (FloatingHeight - hit.distance) / FloatingHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * HoverForce;
            GetComponent<Rigidbody>().AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
    }
}