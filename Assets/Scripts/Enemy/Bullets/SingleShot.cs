using UnityEngine;

[CreateAssetMenu(fileName = "SingleShot", menuName = "WeaponStrategies/SingleShot")]
public class SingleShot : WeaponStrategy
{
    public override void Shoot(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        var audioService = ServiceLocator.Instance.GetService(nameof(AudioService)) as AudioService;
        if (audioService != null)
        {
            audioService.PlaySFX("Single");

        }
        bullet.GetComponent<Bullet>().SetTarget(target); 
    }
}
