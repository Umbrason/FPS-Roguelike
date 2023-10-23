
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    private GameplayControlsInput cached_GameplayControlsInput;
    private GameplayControlsInput GameplayControlsInput => cached_GameplayControlsInput ??= GetComponentInParent<GameplayControlsInput>();

    public float sensitivity = 1f;

    private float rx;
    private float ry;
    void OnEnable() => GameplayControlsInput.Controls.Motion.Look.AddCallback(HandleInput);
    void OnDisable() => GameplayControlsInput.Controls.Motion.Look.RemoveCallback(HandleInput);
    private void HandleInput(InputAction.CallbackContext callbackContext)
    {
        var delta = callbackContext.ReadValue<Vector2>() * sensitivity * .05f;
        rx -= delta.y;
        ry += delta.x;
        rx = Mathf.Clamp(rx, -90, 90);
        ry = (ry + 360f) % 360;
        transform.localRotation = Quaternion.Euler(rx, ry, transform.localEulerAngles.z);
    }
}
