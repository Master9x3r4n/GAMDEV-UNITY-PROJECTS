using UnityEngine;

public class MoveStand : MonoBehaviour
{
    [SerializeField] private float distance = 5f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private TargetHealth trigger;
    
    private void OnEnable()  => trigger.AuraKilledHerself += Stop;
    private void OnDisable() => trigger.AuraKilledHerself -= Stop;

    public void Stop() => enabled = false;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float dir = Mathf.Sign(distance);
        float offset = Mathf.PingPong(Time.time * speed, Mathf.Abs(distance));
        transform.position = startPosition + transform.forward * dir * offset;
    }
}