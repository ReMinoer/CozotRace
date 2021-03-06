﻿using System.Linq;
using DesignPattern;
using UnityEngine;

public class GroundDetectorLine : Factory<GroundDetectorLine>
{
    public int Count = 5;
    public float DetectorsHeight = 5;
    public bool IgnoreFirst;
    public float Interval = 1;
    public GroundDetectorV2[] Detectors { get; private set; }

    public Vector3 Destination
    {
        get { return Detectors.Last().transform.position; }
        set { Interval = (value - transform.position).magnitude / (Count + (IgnoreFirst ? 1 : 0)); }
    }

    private void Start()
    {
        Detectors = new GroundDetectorV2[Count];

        for (int i = 0; i < Count; i++)
        {
            Detectors[i] = GroundDetectorV2.New("GroundDetector");

            Detectors[i].transform.parent = transform;
            Detectors[i].transform.localPosition = transform.forward * ((i + (IgnoreFirst ? 1 : 0)) * Interval);

            var boxCollider = Detectors[i].GetComponent<Collider>() as BoxCollider;
            if (boxCollider != null)
            {
                Vector3 size = boxCollider.size;
                boxCollider.size = new Vector3(size.x, DetectorsHeight, size.z);
            }
        }
    }

    private bool GetNearestObstacle(out Vector3 position)
    {
        foreach (GroundDetectorV2 detector in Detectors)
            if (detector.Ground.IsObstacle)
            {
                position = detector.transform.position;
                return true;
            }

        position = Vector3.zero;
        return false;
    }
}