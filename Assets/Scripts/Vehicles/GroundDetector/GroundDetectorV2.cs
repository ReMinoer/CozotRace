using DesignPattern;
using UnityEngine;

//using TreeEditor;

public class GroundDetectorV2 : Factory<GroundDetectorV2>
{
    private Collider _currentCollider;
    private Vector3 _intersectionPoint;
    public GroundProperty Ground { get; private set; }

    private void Update()
    {
        _intersectionPoint = _currentCollider == null
            ? transform.position
            : _currentCollider.ClosestPointOnBounds(transform.position);
    }

    private void OnTriggerStay(Collider other)
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

    private void OnTriggerExit(Collider other)
    {
        if (other != _currentCollider)
            return;

        Ground = null;
        _currentCollider = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Ground != null && !Ground.IsObstacle ? Color.green : Color.red;
        Gizmos.DrawSphere(_intersectionPoint, 0.1f);
    }
}