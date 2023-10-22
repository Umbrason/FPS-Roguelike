
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class GameplayControlsInput : MonoBehaviour
{
    private PlayerInput cached_playerInput;
    private PlayerInput PlayerInput => cached_playerInput ??= GetComponent<PlayerInput>();
    public GameplayControls cached_Controls { get; set; }
    public GameplayControls Controls
    {
        get
        {
            if (cached_Controls != null) return cached_Controls;
            PlayerInput.actions = (cached_Controls = new GameplayControls()).asset;
            return cached_Controls;
        }
    }

    void OnEnable() => Controls.Enable();
    void OnDisable() => Controls.Disable();
}
