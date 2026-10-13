using UnityEngine;

public class BodyController : MonoBehaviour
{
    [SerializeField] private TargetHealth trigger;

    private void OnEnable()  => trigger.AuraKilledHerself += Stop;
    private void OnDisable() => trigger.AuraKilledHerself -= Stop;
    
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }
    }
    
    private void Stop()
    {
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.AddForce(Vector3.forward * 0.4f, ForceMode.Impulse);
        }
    }
}
