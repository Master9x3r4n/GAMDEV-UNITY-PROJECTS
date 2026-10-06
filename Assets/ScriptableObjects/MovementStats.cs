using UnityEngine;

[CreateAssetMenu(fileName = "MovementStats", menuName = "Scriptable Objects/MovementStats")]
public class MovementStats : ScriptableObject
{
    public float speed = 8f;
    public float turnSpeed = 7f;
    public float jumpHeight = 14f;
    public float fallMultiplier = 3.5f;
}
