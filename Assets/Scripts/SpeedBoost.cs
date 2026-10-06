using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    [SerializeField] MovementStats movementStats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            movementStats.currSpeed += 2;
        }
        Destroy(gameObject);
    }
}
