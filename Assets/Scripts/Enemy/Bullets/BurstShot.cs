using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BurstShot", menuName = "WeaponStrategies/BurstShot")]
public class BurstShot : WeaponStrategy
{
    public int burstCount = 3;    // Número de balas por ráfaga
    public float burstDelay = 0.5f; // Tiempo de espera entre disparos en la ráfaga

    public override void Shoot(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        // Iniciar la coroutine desde el script adjunto al shootPoint
        shootPoint.GetComponent<ShootPointController>().StartCoroutine(BurstFire(shootPoint, bulletPrefab, target));
    }

    private IEnumerator BurstFire(Transform shootPoint, GameObject bulletPrefab, Transform target)
    {
        for (int i = 0; i < burstCount; i++)
        {
            // Instanciar la bala
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            bullet.GetComponent<Bullet>().SetTarget(target);

            // Esperar antes de instanciar la siguiente bala
            yield return new WaitForSeconds(burstDelay);
        }
    }
}