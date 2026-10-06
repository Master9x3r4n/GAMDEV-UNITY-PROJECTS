using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
        if (ph != null)
        {
            ph.IncreaseScore();
            Destroy(this.gameObject); 
        }
        
    }

    void OnEnable()
    {
        GameManager.OnGameOver += Die;
    }

    void OnDisable()
    {
        GameManager.OnGameOver -= Die;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    void Start()
    {
        startPos = transform.position;
        rotateDirection = Random.Range(0, 2) * 2 - 1;
    }
    
    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        
        transform.Rotate(Vector3.forward * rotateDirection * rotateSpeed * Time.deltaTime);
    }
}