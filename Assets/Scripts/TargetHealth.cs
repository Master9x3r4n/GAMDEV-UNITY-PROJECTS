using System;
using UnityEngine;

public class TargetHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public int maxHits = 5;
    private int currentHits;

    [Header("Physics Settings")]
    [SerializeField] private float minVelocity = 20.5f;
    [SerializeField] private float maxVelocity = 21.2f;

    private HingeJoint hinge;
    private Rigidbody rb;
    private bool knockedOut;
    
    public event Action AuraKilledHerself;
    public static event Action AnyAuraKilledHerself;

    void Start()
    {
        currentHits = maxHits;
        hinge = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (minVelocity < collision.relativeVelocity.magnitude && collision.relativeVelocity.magnitude < maxVelocity)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHits--;
        if (currentHits <= 0)
        {
            Knockout();
        }
    }

    private void Knockout()
    {
        if (knockedOut || hinge == null) return;
        knockedOut = true;

        Destroy(hinge);
        rb.AddForce(Vector3.up * 1.75f, ForceMode.Impulse);
        Raise();
    }
    
    private void Raise()
    {
        AuraKilledHerself?.Invoke();
        AnyAuraKilledHerself?.Invoke();
    }
}