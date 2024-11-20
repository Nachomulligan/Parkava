using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    public WeaponStrategy weaponStrategy;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    public BulletMovementStrategy defaultMovementStrategy;
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

    InjectableCommand _killCommand;

    //private void OnEnable()
    //{
    //    CommandConsoleService console = null; //Get from service locator
    //    _killCommand = new InjectableCommand($"Kill {name}", Die, DieWithArguments);
    //    console.AddCommand(_killCommand);
    //}
    //private void OnDisable()
    //{
    //    CommandConsoleService console = null; //Get from service locator
    //    console.RemoveCommand(_killCommand);
    //}

    private void Start()
    {
        initialRotation = transform.rotation;

        currentWaypoint = waypointPath.GetWayPoint(currentWaypointIndex);
        nextWaypoint = waypointPath.GetWayPoint(waypointPath.GetNextWaypointIndex(currentWaypointIndex));
    }

    private void Update()
    {

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
        while (!destroyCancellationToken.IsCancellationRequested)
        {
            GameObject bulletObject = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
            Bullet bullet = bulletObject.GetComponent<Bullet>();

            if (bullet != null && defaultMovementStrategy != null)
            {
                bullet.SetMovementStrategy(defaultMovementStrategy);
                bullet.SetTarget(player);
            }

            yield return new WaitForSeconds(shootPeriod);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    private void DieWithArguments(string[] obj)
    {
        throw new NotImplementedException();
    }
    public void Die()
    {

    }
}