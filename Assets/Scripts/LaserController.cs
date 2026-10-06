using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserController : MonoBehaviour
{
    private float laserSpeed = 10f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laserSpeed = Random.Range(10f, 20f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.left * laserSpeed * Time.deltaTime;
        if (transform.position.x < -2.5f) Destroy(gameObject);
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHandler player = other.GetComponent<PlayerHandler>();
            player.ReduceHealth();
        }
    }

    void OnGameOver()
    {
        Destroy(gameObject);
    }
    
    
}
