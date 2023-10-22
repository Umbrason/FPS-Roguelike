using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchController : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint spine;
    [SerializeField] private Rigidbody headRB;
    [SerializeField] private Collider headCollider;
    [SerializeField] private Collider neckCollider;
    public float crouchLen;
    public float standingLen;
    bool crouching = false;


    private GameplayControlsInput cached_GameplayControlsInput;
    private GameplayControlsInput GameplayControlsInput => cached_GameplayControlsInput ??= GetComponentInParent<GameplayControlsInput>();
    void OnEnable()
    {
        GameplayControlsInput.Controls.Motion.Crouch.AddCallback(SetInput);
        headRB.sleepThreshold = -1f;
    }

    void OnDisable()
    {
        GameplayControlsInput.Controls.Motion.Crouch.RemoveCallback(SetInput);
    }
    private void SetInput(InputAction.CallbackContext callbackContext) => crouching = callbackContext.ReadValueAsButton();

    void FixedUpdate()
    {
        var spineLen = crouching ? crouchLen : standingLen;
        spine.connectedAnchor = transform.TransformPoint(Vector3.up * spineLen);
        neckCollider.enabled = headCollider.enabled = !(spine.transform.localPosition.y > spineLen + .1f); //no decapitation
    }
}
