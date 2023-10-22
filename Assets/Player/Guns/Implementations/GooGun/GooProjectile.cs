using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class GooProjectile : MonoBehaviour
{
    private Transform stick;
    private Vector3 stickOffset;
    private Quaternion stickRotationOffset;

    private Projectile cached_Projectile;
    public Projectile Projectile => cached_Projectile ??= GetComponent<Projectile>();

    void OnCollisionEnter(Collision collision)
    {
        if (this.destroyed) return;
        var goo = collision.gameObject.GetComponent<GooProjectile>();
        if (goo != null && !goo.destroyed && goo.Mass <= 1f)
        {
            Destroy(gameObject);
            destroyed = true;
            goo.Grow(this);
            return;
        }

        if (stick != null) return;

        var averageContactPos = Vector3.zero;
        foreach (var contact in collision.contacts)
            averageContactPos += contact.point;
        Projectile.visual.transform.localPosition = Vector3.zero;
        Projectile.enabled = false;
        stick = collision.gameObject.transform;
        stickOffset = stick.InverseTransformPoint(averageContactPos);
        stickRotationOffset = stick.worldToLocalMatrix.rotation * transform.rotation;
        gameObject.layer = GameLayers.PlayerProjectiles;
        Projectile.RB.constraints = RigidbodyConstraints.FreezeAll;
    }

    private bool destroyed;
    private float Mass = 1f;
    public void Grow(GooProjectile other)
    {
        var weightOther = Mathf.Pow(other.Mass, 3f);
        var weightSelf = Mathf.Pow(Mass, 3f);
        stickOffset = (other.stickOffset * weightOther + stickOffset * weightSelf) / (weightOther + weightSelf);
        this.Mass += other.Mass;
        transform.localScale = Vector3.one * Mathf.Pow(this.Mass, 1 / 3f);
    }

    void Update()
    {
        if (stick == null) return;
        transform.position = stick.TransformPoint(stickOffset);
        transform.rotation = stick.rotation * stickRotationOffset;
    }
}
