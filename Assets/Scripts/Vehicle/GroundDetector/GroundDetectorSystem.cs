using System;
using UnityEngine;

public class GroundDetectorSystem : MonoBehaviour
{
    public int LateralCount = 5;
	public float Interval = 1;
	public float Angle = 15;
    public float DetectorsHeight = 2;

	public GroundDetectorV2 Central { get; private set; }
	public GroundDetectorLine Left { get; private set; }
	public GroundDetectorLine Right { get; private set; }

	void Awake()
	{
        Central = GroundDetectorV2.New("GroundDetector");
		Central.transform.parent = transform;
		Central.transform.localPosition = Vector3.zero;

		Left = GroundDetectorLine.New();
		Left.Count = LateralCount;
		Left.Interval = Interval;
		Left.DetectorsHeight = DetectorsHeight;
		Left.IgnoreFirst = true;

		Left.transform.parent = transform;
		Left.transform.localPosition = Vector3.zero;
		Left.transform.rotation = Quaternion.AngleAxis(-Angle, transform.up);
		
		Right = GroundDetectorLine.New();
		Right.Count = LateralCount;
		Right.Interval = Interval;
		Right.DetectorsHeight = DetectorsHeight;
		Right.IgnoreFirst = true;

		Right.transform.parent = transform;
		Right.transform.localPosition = Vector3.zero;
		Right.transform.rotation = Quaternion.AngleAxis(Angle, transform.up);
	}
}
