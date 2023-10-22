
using UnityEngine;

public class CursorLockManager : MonoBehaviour
{
    public void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
