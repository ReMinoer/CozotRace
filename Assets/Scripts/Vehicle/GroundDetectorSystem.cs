using System;
using UnityEngine;

public class GroundDetectorSystem : MonoBehaviour
{
    public int LateralCount = 5;
	public float Interval = 0.2f;
	public float Angle = 30;
    public float DetectorsHeight = 2;

    public GroundDetectorV2 Central { get; private set; }
    public GroundDetectorV2[] Left { get; private set; }
    public GroundDetectorV2[] Right { get; private set; }

	void Awake()
	{
        Central = GroundDetectorV2.New("GroundDetector");
        Central.transform.parent = transform;
        Central.transform.localPosition = Vector3.zero;

	    var boxCollider = Central.collider as BoxCollider;
	    if (boxCollider == null)
            throw new NullReferenceException();

	    Vector3 size = boxCollider.size;

	    Left = new GroundDetectorV2[LateralCount];
        Right = new GroundDetectorV2[LateralCount];

	    for (int i = 0; i < LateralCount; i++)
        {
            Left[i] = GroundDetectorV2.New("GroundDetector");
            Right[i] = GroundDetectorV2.New("GroundDetector");

            Left[i].transform.parent = transform;
            Left[i].transform.localPosition = Quaternion.AngleAxis(-Angle, Vector3.up) * -transform.forward * ((i + 1) * Interval);

            boxCollider = Left[i].collider as BoxCollider;
            if (boxCollider != null)
                boxCollider.size = new Vector3(size.x, DetectorsHeight, size.z);

            Right[i].transform.parent = transform;
            Right[i].transform.localPosition = Quaternion.AngleAxis(Angle, Vector3.up) * -transform.forward * ((i + 1) * Interval);

            boxCollider = Right[i].collider as BoxCollider;
            if (boxCollider != null)
                boxCollider.size = new Vector3(size.x, DetectorsHeight, size.z);
	    }
	}
}
