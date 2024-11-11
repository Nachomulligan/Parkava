using UnityEngine;

[CreateAssetMenu(fileName = "SingleShot", menuName = "WeaponStrategies/SingleShot")]
public class SingleShot : WeaponStrategy
{
    public override void Shoot(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<Bullet>().SetTarget(target); 
    }
}
