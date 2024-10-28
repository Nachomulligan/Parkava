using UnityEngine;

[CreateAssetMenu(fileName = "SingleShot", menuName = "WeaponStrategies/SingleShot")]
public class SingleShot : WeaponStrategy
{
    public override void Shoot(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        // Asegúrate de que el target esté actualizado antes de cada disparo
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        bullet.GetComponent<Bullet>().SetTarget(target); // Actualiza siempre con la nueva posición del target
    }
}
