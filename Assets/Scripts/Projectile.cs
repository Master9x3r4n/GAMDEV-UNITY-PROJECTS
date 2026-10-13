using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 20f;

    private void Start()
    {
        GetComponent<Rigidbody>().linearVelocity = transform.forward * speed;
        GetComponent<Rigidbody>().useGravity = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}