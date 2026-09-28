using UnityEngine;

public class Coin : MonoBehaviour
{   
    private int rotateDirection;
    [SerializeField] private int rotateSpeed = 100;
    [SerializeField] private float speed = 1.0f;  
    [SerializeField] private float height = 0.2f; 
    
    private Vector3 startPos;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler ph = other.GetComponent<PlayerHandler>();
        
        // verify its the player if it has PlayerHandler script
        if (ph != null)
        {
            print("YOU WINNNN!!!");
            Destroy(this.gameObject); //delete this object
        }
        
    }

    void Start()
    {
        startPos = transform.position;
        rotateDirection = Random.Range(0, 2) * 2 - 1;
    }
    
    // Rotate in a direction
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        
        transform.Rotate(Vector3.up * rotateDirection * rotateSpeed * Time.deltaTime);
    }
}