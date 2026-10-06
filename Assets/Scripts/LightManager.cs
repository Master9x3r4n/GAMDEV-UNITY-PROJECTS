using UnityEngine;
using System.Collections;

public class LightManager : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2.5f;
    [SerializeField] private float startAngle = -70f;
    [SerializeField] private float targetAngle = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.rotation = Quaternion.Euler(startAngle, 0f, 0f);
    }

    void OnEnable()
    {
        GameManager.OnGameOver += RotateSun;
    }

    void OnDisable()
    {
        GameManager.OnGameOver -= RotateSun;
    }

    private void RotateSun()
    {
        float currentAngle = startAngle;
        
        while (!Mathf.Approximately(currentAngle, targetAngle))
        {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(currentAngle, 0f, 0f);
        }
        
        transform.rotation = Quaternion.Euler(targetAngle, 0f, 0f);
    }
}
