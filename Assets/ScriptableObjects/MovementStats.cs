using UnityEngine;

[CreateAssetMenu(fileName = "MovementStats", menuName = "Scriptable Objects/MovementStats")]
public class MovementStats : ScriptableObject
{
    [System.NonSerialized]
    public float currSpeed = 8f;
    [System.NonSerialized]
    public float currTurnSpeed = 7f;
    public float jumpHeight = 16f;
    public float fallMultiplier = 3.5f;
    public float speed = 8f;
    public float turnSpeed = 7f;
    
}
