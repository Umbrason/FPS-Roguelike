using UnityEngine;

public class GunAnimatorInterface : MonoBehaviour
{
    private Animator cached_animator;
    private Animator Animator => cached_animator ??= GetComponentInChildren<Animator>();
    [field: SerializeField] public Transform Nozzle { get; private set; }
    
    public void Trigger()
    {
        Animator.SetTrigger("Fire");
    }

    public float VelRX { get; set; }
    public float VelRY { get; set; }
    public float VelY { get; set; }

}
