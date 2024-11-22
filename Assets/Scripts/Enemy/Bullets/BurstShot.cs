using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstShot", menuName = "WeaponStrategies/BurstShot")]
public class BurstShot : WeaponStrategy
{
    public int burstCount = 3;
    public float burstDelay = 0.5f;

    public override void Shoot(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        shootPoint.GetComponent<ShootPointController>().StartCoroutine(BurstFire(shootPoint, bulletPrefab, target));
    }

    private IEnumerator BurstFire(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        for (int i = 0; i < burstCount; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Bullet>().SetTarget(target);

            yield return new WaitForSeconds(burstDelay);
        }
    }
}