using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;
    
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 targetPosition = cameraTarget.position + (cameraTarget.rotation * offset);
        
        // look at player target
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Vector3 lookTarget = cameraTarget.position + Vector3.up * offset.y;
        transform.LookAt(lookTarget);
    }
}
