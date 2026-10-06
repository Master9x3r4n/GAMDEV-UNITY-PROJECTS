using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab; 
    [SerializeField] private int coinAmount = 10;
    
    void Start()
    {
        BoxCollider groundCollider =  GetComponent<BoxCollider>();
        for (int j = 0; j < coinAmount; j++)
        {
            Vector3 startPos = RandomPointInBounds(groundCollider.bounds);
            Quaternion rotate = Quaternion.Euler(90, Random.Range(0f, 360f), 0);
            Instantiate(coinPrefab, startPos, rotate); 
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x), 
            Random.Range(-0.6f, 4f),
            Random.Range(bounds.min.z, bounds.max.z) 
        );
    }
}