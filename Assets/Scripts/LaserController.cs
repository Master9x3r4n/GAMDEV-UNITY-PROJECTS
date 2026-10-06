// LaserController.cs
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LaserController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 15f;

    private float laserSpeed = 10f;
    private int rotateDirection;

    public bool IsRotating { get; set; }

    void Start()
    {
        laserSpeed = Random.Range(10f, 30f);
        rotateDirection = Random.Range(0, 2) * 2 - 1;
    }

    void Update()
    {
        transform.position += Vector3.left * laserSpeed * Time.deltaTime;

        if (IsRotating)
        {
            transform.Rotate(Vector3.right, rotateDirection * rotationSpeed * Time.deltaTime);
        }

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