using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{
    public Transform GetWayPoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    // Update is called once per frame
    public int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWayPointIndex = currentWaypointIndex + 1;

        if (nextWayPointIndex == transform.childCount)
        {
            nextWayPointIndex = 0;
        }
        return nextWayPointIndex;
    }
}
