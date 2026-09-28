using System;
using UnityEngine;

public class CheckpointHandler : MonoBehaviour
{
    [SerializeField] private Vector3 checkpoint;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerHandler ph = other.GetComponent<PlayerHandler>();
        if (ph != null)
        {
            print("Checkpoint set!");
            ph.SetOrigin(checkpoint);
        }
}
}
