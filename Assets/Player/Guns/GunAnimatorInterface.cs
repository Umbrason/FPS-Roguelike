using UnityEditor;
using UnityEngine;

public class GunAnimatorInterface : MonoBehaviour
{
    private Cached<Animator> cached_animator;
    private Animator Animator => cached_animator[this];
    [field: SerializeField] public Transform Nozzle { get; private set; }

    private void Start()
    {
        GunRotationSpring = new(config);
    }
    

    private void Update()
    {
        GunRotationSpring.RestingPos = transform.position + transform.forward * 2f;
        GunRotationSpring.Step(Time.deltaTime);
        var localVelocity = transform.InverseTransformVector(GunRotationSpring.Velocity);
        Animator.SetFloat("VX", localVelocity.x / 15f);
        Animator.SetFloat("VY", localVelocity.y / 15f);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying && GunRotationSpring != null)
        {
            var targetPos = transform.position + transform.forward * .1f;
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(targetPos, Vector3.one * .2f);
            Gizmos.color = Color.white;
            Gizmos.DrawWireSphere(GunRotationSpring.Position, .2f);
        }
    }
#endif

    public void Trigger()
    {
        Animator.SetTrigger("Fire");
    }

    public Spring.Config config;
    Vector3Spring GunRotationSpring;
}
