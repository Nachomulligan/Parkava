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
    private bool isShooting;

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