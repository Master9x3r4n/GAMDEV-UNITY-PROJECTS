using UnityEngine;

public class TriggerPlatform : MonoBehaviour
{
    [SerializeField] private Vector3 finalPos;
    [SerializeField] private float speed = 0f;
    
    [SerializeField] private Vector3 rotateBy = new Vector3(0f, 0f, 0f);
    [SerializeField] private float rotateSpeed = 0f;
    
    private bool isActive = false;
    private Quaternion startRot;
    private Quaternion targetRot;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isActive = false;
        
        startRot = transform.rotation;
        targetRot = startRot * Quaternion.Euler(rotateBy);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPos, speed * Time.deltaTime);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
    }

    public void ActivatePlatform()
    {
        isActive = true;
    }
}
