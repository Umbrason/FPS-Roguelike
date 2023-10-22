using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Guns/GooGun")]
public class GooGun : Gun
{
    [SerializeField] Projectile projectile;
    [SerializeField] float speed = 50f;

    public override void ShootBullet(Vector3 nozzlePosition, Ray ray, int index)
    {
        var projectileInstance = Instantiate(projectile, ray.origin, Quaternion.LookRotation(ray.direction));
        projectileInstance.Init(ray.origin, nozzlePosition, ray.direction, speed);
    }
}
