using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private bool followPosition;
    [SerializeField] private bool followRotation;
    [SerializeField] private Transform followTarget;
    void LateUpdate()
    {
        if (followPosition) transform.position = followTarget.position;
        if (followRotation) transform.rotation = followTarget.rotation;
    }
}
