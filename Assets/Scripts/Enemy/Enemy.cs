using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public WeaponStrategy weaponStrategy;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public Transform player;
    [SerializeField] private float shootPeriod = 1;
    [SerializeField] private float attackRange = 10;
    [SerializeField] private WaypointPath waypointPath;
    [SerializeField] private float movementSpeed = 2f;
    private bool isShooting;
    private int currentWaypointIndex = 0;
    private Transform currentWaypoint;
    private Transform nextWaypoint;
    private Quaternion initialRotation;

    private void Start()
    {
        initialRotation = transform.rotation;

        currentWaypoint = waypointPath.GetWayPoint(currentWaypointIndex);
        nextWaypoint = waypointPath.GetWayPoint(waypointPath.GetNextWaypointIndex(currentWaypointIndex));
    }

    private void Update()
    {
        MoveAlongPath();

        if (Vector3.Distance(transform.position, player.position) < attackRange)
        {
            if (!isShooting)
            {
                StartCoroutine(Shoot());
                isShooting = true;
            }
        }
        else
        {
            StopCoroutine(Shoot());
            isShooting = false;
        }
    }

    private void MoveAlongPath()
    {

        transform.position = Vector3.MoveTowards(transform.position, nextWaypoint.position, movementSpeed * Time.deltaTime);
        transform.rotation = initialRotation;

        if (Vector3.Distance(transform.position, nextWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = waypointPath.GetNextWaypointIndex(currentWaypointIndex);
            nextWaypoint = waypointPath.GetWayPoint(currentWaypointIndex);
        }
    }

    private IEnumerator Shoot()
    {
        //!destroyCancelationToken.IsCancellationRequested)
        while (true)
        {
            weaponStrategy.Shoot(shootPoint, bulletPrefab, player);
            yield return new WaitForSeconds(shootPeriod);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}