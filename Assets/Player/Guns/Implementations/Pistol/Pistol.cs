using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Gun
{
    [SerializeField] private float environmentBulletRadius = 0f;
    [SerializeField] private float neutralEntityHitboxRadius = .5f;
    [SerializeField] private float enemyEntityHitboxBulletRadius = 1f;

    [SerializeField] private LineRenderer SmokeTrailPrefab;

    public override void ShootBullet(Vector3 nozzlePosition, Ray ray, int index)
    {
        var hasHit = Hittests.GenerousSpherecast(ray, out var hit,
        new(GameLayers.AsBitmask(GameLayers.EnemyEntitiesCriticalZone), enemyEntityHitboxBulletRadius),
        new(GameLayers.AsBitmask(GameLayers.EnemyEntities), enemyEntityHitboxBulletRadius),
        new(GameLayers.AsBitmask(GameLayers.NeutralEntitiesCriticalZone), neutralEntityHitboxRadius),
        new(GameLayers.AsBitmask(GameLayers.NeutralEntities), neutralEntityHitboxRadius),
        new(GameLayers.AsBitmask(GameLayers.Environment), environmentBulletRadius));
        SpawnVFX(nozzlePosition, hasHit ? hit.point : ray.GetPoint(1000f), hit, hasHit);
    }

    private void SpawnVFX(Vector3 from, Vector3 to, RaycastHit hit, bool isHit)
    {
        Debug.DrawLine(from, to);
        if (isHit) Debug.DrawRay(hit.point, hit.normal);
        var lr = Instantiate(SmokeTrailPrefab);
        lr.SetPosition(0, from);
        lr.SetPosition(1, to);
        Destroy(lr.gameObject, .4f);
    }
}

