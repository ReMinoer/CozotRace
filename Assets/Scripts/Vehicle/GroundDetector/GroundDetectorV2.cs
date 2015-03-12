using DesignPattern;
using TreeEditor;
using UnityEngine;

public class GroundDetectorV2 : Factory<GroundDetectorV2>
{
    public GroundProperty Ground { get; private set; }

    private Collider _currentCollider;
    private Vector3 _intersectionPoint;

    void Update()
    {
        _intersectionPoint = _currentCollider == null
            ? transform.position
            : _currentCollider.ClosestPointOnBounds(transform.position);
    }

    void OnTriggerStay(Collider other)
    {
        if (other == _currentCollider)
            return;

        foreach (GroundProperty ground in Map.Instance.Grounds)
            if (other.gameObject.GetComponent<Renderer>().material.mainTexture == ground.Texture && Ground == null)
            {
                Ground = ground;
                _currentCollider = other;
                return;
            }
	}

    void OnTriggerExit(Collider other)
    {
        if (other != _currentCollider)
            return;

        Ground = null;
        _currentCollider = null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Ground != null && !Ground.IsObstacle ? Color.green : Color.red;
        Gizmos.DrawSphere(_intersectionPoint, 0.1f);
    }
}
