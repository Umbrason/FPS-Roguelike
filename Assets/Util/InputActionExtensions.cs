
using System;
using UnityEngine.InputSystem;

public static class InputActionExtensions
{
    public static void AddCallback(this InputAction inputAction, Action<InputAction.CallbackContext> action)
    {
        inputAction.started += action;
        inputAction.performed += action;
        inputAction.canceled += action;
    }

    public static void RemoveCallback(this InputAction inputAction, Action<InputAction.CallbackContext> action)
    {
        inputAction.started -= action;
        inputAction.performed -= action;
        inputAction.canceled -= action;
    }
}