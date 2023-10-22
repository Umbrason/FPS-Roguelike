using UnityEngine;

public static class NozzlePositionCorrection
{
    public static Vector3 NozzleWorldPosition(Transform NozzleTransform, Camera InWorldCam, Camera WeaponRigCam)
    {
        var weaponRigNozzleScreenPoint = WeaponRigCam.WorldToScreenPoint(NozzleTransform.position);
        var inWorldNozzlePosition = InWorldCam.ScreenToWorldPoint(weaponRigNozzleScreenPoint);
        return (Vector3)inWorldNozzlePosition;
    }

}
