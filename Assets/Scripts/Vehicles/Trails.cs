using UnityEngine;

public class Trails : MonoBehaviour
{
    public float Seuil;
    protected Vector3 Velocity2D;

    private void Start()
    {
        GetComponent<TrailRenderer>().enabled = false;
    }

    private void Update()
    {
        Velocity2D = gameObject.GetComponentInParent<Rigidbody>().velocity;
        Velocity2D.y = 0;
        float speed = Velocity2D.magnitude;
        GetComponent<TrailRenderer>().enabled = speed >= Seuil;
    }
}