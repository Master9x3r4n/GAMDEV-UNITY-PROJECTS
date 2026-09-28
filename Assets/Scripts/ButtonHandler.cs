using UnityEngine;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] private TriggerPlatform target;
    private float originalY;
    private bool isTriggered;
    
    void Start()
    {
        isTriggered = false;
        originalY = transform.position.y;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        transform.position = new Vector3(transform.position.x, originalY - 0.1f, transform.position.z);

        if (!isTriggered && target != null)
        {
            isTriggered = true;
            target.ActivatePlatform();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
    }
}
