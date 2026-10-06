using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform cameraTarget;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.25f;
    
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        Vector3 targetPosition = new Vector3(
            cameraTarget.position.x + offset.x,
            transform.position.y,
            transform.position.z
        );
        
        // look at player target
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        Vector3 lookTarget = new Vector3(
            cameraTarget.position.x,
            transform.position.y - offset.y, 
            transform.position.z + offset.z
        );
        transform.LookAt(lookTarget);
    }
}
