
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CollisionInfo), typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField] private int jumpCount;
    [SerializeField] private float CoyoteTime = .1f;

    [SerializeField] private float JumpHeight;

    private Rigidbody cached_RB;
    private Rigidbody RB => cached_RB ??= GetComponent<Rigidbody>();

    private CollisionInfo cached_CI;
    private CollisionInfo CI => cached_CI ??= GetComponent<CollisionInfo>();
    
    private GameplayControlsInput cached_GameplayControlsInput;
    private GameplayControlsInput GameplayControlsInput => cached_GameplayControlsInput ??= GetComponentInParent<GameplayControlsInput>();

    void OnEnable() => GameplayControlsInput.Controls.Motion.Jump.AddCallback(SetInput);
    void OnDisable() => GameplayControlsInput.Controls.Motion.Jump.RemoveCallback(SetInput);
    private bool JumpInput;
    public void SetInput(InputAction.CallbackContext callback) => JumpInput = callback.ReadValueAsButton();

    private int jumpsPerformed;
    void DoJump()
    {
        if (jumpCount < jumpsPerformed + 1) return;
        if (CoyoteTime < Time.fixedTime - lastGroundTime) jumpsPerformed++;
        var velocity = Mathf.Sqrt(2 * -Physics.gravity.y * JumpHeight);
        RB.velocity = RB.velocity._x0z() + Vector3.up * Mathf.Max(velocity, RB.velocity.y);
        lastGroundTime = float.MinValue;
    }

    void JumpInputChanged(bool jumpInput)
    {
        this.JumpInput = jumpInput;
        if (jumpInput) DoJump();
    }

    private float lastGroundTime;
    void FixedUpdate()
    {
        if (!CI.FlatGround) return;
        lastGroundTime = Time.fixedTime;
        if (JumpInput) DoJump();
        jumpsPerformed = 0;
    }


}
