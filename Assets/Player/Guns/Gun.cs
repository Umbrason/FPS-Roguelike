using System;
using System.Collections;
using UnityEngine;

public abstract class Gun : ScriptableObject
{
    public GunProfile gunProfile;    
    public bool canFire { get; private set; } = true;

    public IEnumerator FireRoutine(Vector3 nozzlePosition, Camera cam, GunAnimatorInterface gunAnimator)
    {
        canFire = false;
        var ray = cam.ViewportPointToRay(Vector2.one / 2f);
        for (int i = 0; i < gunProfile.burstCount; i++)
        {
            gunAnimator.Trigger();
            for (int j = 0; j < gunProfile.burstSize; j++)
            {
                var bulletIndex = i * gunProfile.burstSize + j;
                var bulletRay = ray;
                AddSpread(ref bulletRay, gunProfile.spread);
                ShootBullet(nozzlePosition, bulletRay, bulletIndex);
            }
            yield return new WaitForSeconds(gunProfile.burstDelay);
        }
        yield return new WaitForSeconds(1f / gunProfile.firerate);
        canFire = true;
    }

    private void AddSpread(ref Ray ray, float maxAngle)
    {
        var dir = ray.direction.normalized;
        var offset = Quaternion.FromToRotation(Vector3.forward, dir) * UnityEngine.Random.insideUnitCircle;
        var amplitude = Mathf.Tan(maxAngle * Mathf.Deg2Rad);
        ray.direction = (dir + offset * amplitude).normalized;
    }

    public abstract void ShootBullet(Vector3 nozzlePosition, Ray ray, int index);

    [Serializable]
    public struct GunProfile
    {
        //need a graph or something
        //not sufficient for e.g.
        // - gun that increases firerate/spread over time/bursts

        [Tooltip("How many bursts to fire per trigger click")] public int burstCount;
        [Tooltip("How many bullets to fire per burst")] public int burstSize;
        [Tooltip("How long to wait between bursts")] public float burstDelay;
        [Tooltip("1 / minimum delay between trigger clicks")] public float firerate;
        [Tooltip("How much recoil to add after each shot burst")] public float recoilPerBurst;
        [Tooltip("How much spread to add to each bullet")] public float spread;
        public GunAnimatorInterface GunVisualPrefab;
    }
}