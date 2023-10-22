using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class GunEquipmentSystem : MonoBehaviour
{
    public Gun SelectedGun;
    public Camera inWorldCam;
    public Transform fpsRigRoot;
    private Camera fpsRigCam;

    private GameplayControlsInput cached_GameplayControlsInput;
    private GameplayControlsInput GameplayControlsInput => cached_GameplayControlsInput ??= GetComponentInParent<GameplayControlsInput>();

    private bool FireInput;
    void OnEnable() => GameplayControlsInput.Controls.Gunplay.Fire.AddCallback(SetInput);
    void OnDisable() => GameplayControlsInput.Controls.Gunplay.Fire.RemoveCallback(SetInput);
    private void SetInput(InputAction.CallbackContext callbackContext) => FireInput = callbackContext.ReadValueAsButton();


    void Start()
    {
        ReinstantiateGunVisual();
    }

    void OnDestroy()
    {
        if (gunVisualInstance != null) Destroy(gunVisualInstance.gameObject);
    }


    private GunAnimatorInterface gunVisualInstance;
    private void ReinstantiateGunVisual()
    {
        if (gunVisualInstance != null)
        {
            inWorldCam.GetUniversalAdditionalCameraData().cameraStack.Remove(fpsRigCam);
            Destroy(gunVisualInstance.gameObject);
        }
        gunVisualInstance = Instantiate(SelectedGun.gunProfile.GunVisualPrefab, fpsRigRoot, false);
        fpsRigCam = gunVisualInstance.GetComponentInChildren<Camera>();
        inWorldCam.GetUniversalAdditionalCameraData().cameraStack.Add(fpsRigCam);
    }

    public void TryFire()
    {
        var actualNozzlePosition = NozzlePositionCorrection.NozzleWorldPosition(gunVisualInstance.Nozzle, inWorldCam, fpsRigCam);
        if (SelectedGun.canFire) StartCoroutine(SelectedGun.FireRoutine(actualNozzlePosition, inWorldCam, gunVisualInstance));
    }

    void Update()
    {
        if (FireInput) TryFire();
    }
}
