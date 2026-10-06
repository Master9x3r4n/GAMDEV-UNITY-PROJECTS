using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class LaserSpawner : MonoBehaviour
{
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float minInterval = 1.5f;
    [SerializeField] private float maxInterval = 3.5f;
    [SerializeField] private int level = 1;
    
    private float spawnOffset = -10f;
    private bool isActive = false;
    private Coroutine spawnCoroutine;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (level == 1)
        {
            StartSpawning();
        }
    }

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameStart += OnGameStart;
        GameManager.LevelUp += OnLevelUp;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameStart -= OnGameStart;
        GameManager.LevelUp -= OnLevelUp;
        StopSpawning();
    }

    private void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            isActive = true;
            spawnCoroutine = StartCoroutine(SpawnLaser());
        }
    }

    private void StopSpawning()
    {
        isActive = false;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator SpawnLaser()
    {
        while (true)
        {
            if (isActive)
            {
                Quaternion rotate = Quaternion.Euler(Random.Range(0f, 360f), 0, 0);
                Vector3 position = new Vector3(spawnOffset, Random.Range(-1.5f, 2.5f), Random.Range(-2.5f, 2.5f));
                Instantiate(laserPrefab, position + transform.position, rotate);
                yield return new WaitForSeconds(Random.Range(minInterval, maxInterval));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isActive = false;
        if (other.CompareTag("Player"))
        {
            StopSpawning();
            
            PlayerHandler player = other.GetComponent<PlayerHandler>();
            if (player != null)
            {
                player.LevelUp();
            }
        }
    }

    private void OnGameStart()
    {
        if (level == 1)
        {
            StartSpawning();
        }
        else
        {
            StopSpawning();
        }
    }

    private void OnGameOver()
    {
        StopSpawning();
    }
    
    private void OnLevelUp(int currentLevel)
    {
        if (this.level == currentLevel)
        {
            StartSpawning();
        }
        else
        {
            StopSpawning();
        }
    }
}
