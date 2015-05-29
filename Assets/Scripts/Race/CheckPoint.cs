using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CheckPoint : LinkedPoint
{
    protected override void Awake()
    {
        if (FindObjectOfType<Race>() != null)
            base.Awake();
    }

    protected override GameObject GetLastPoint()
    {
        return Race.Instance.GetLastCheckPoint();
    }

    protected override List<GameObject> GetAllPoints()
    {
        return Race.Instance.GetAllPoints();
    }

    protected override void DisableInsertModeOtherPoints(GameObject activePoint)
    {
        Race.Instance.DisableInsertModeOtherPoints(activePoint);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.isTrigger && col.gameObject.GetComponent<Contestant>() != null)
            col.gameObject.GetComponent<Contestant>().ValidateCheckPoint(this);
    }

    private void OnDrawGizmos()
    {
        if (NextPoint != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, NextPoint.transform.position);
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, Race.Instance.FirstCheckPoint.transform.position);
        }
    }
}