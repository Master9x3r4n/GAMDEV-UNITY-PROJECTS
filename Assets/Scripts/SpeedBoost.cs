using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] MovementStats movementStats;
    [SerializeField] private float speedBonus = 2f;
    [SerializeField] private float revealPeriod = 0.15f;

    private Renderer[] renderers;
    private float lastLitTime = float.NegativeInfinity;
    private bool isRevealed;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
    }

    void Start()
    {
        SetVisible(false);
    }

    void Update()
    {
        bool shouldBeRevealed = Time.time - lastLitTime <= revealPeriod;

        if (shouldBeRevealed != isRevealed)
        {
            isRevealed = shouldBeRevealed;
            SetVisible(isRevealed);
        }
    }


    public void Reveal()
    {
        lastLitTime = Time.time;
        
        if (!isRevealed)
        {
            isRevealed = true;
            SetVisible(true);
        }
    }

    private void SetVisible(bool visible)
    {
        foreach (Renderer r in renderers)
        {
            if (r != null) r.enabled = visible;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        TryCollect(other);
    }
    
    void OnTriggerStay(Collider other)
    {
        TryCollect(other);
    }

    private void TryCollect(Collider other)
    {
        Debug.Log($"revealed={isRevealed}");
        

        if (!isRevealed) return;

        bool isPlayer = other.CompareTag("Player") ||
                        (other.attachedRigidbody != null && other.attachedRigidbody.CompareTag("Player"));
        if (!isPlayer) return;

        movementStats.currSpeed += speedBonus;
        Destroy(gameObject);
    }
}