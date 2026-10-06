using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public int maxHealth = 5;
    public Vector3 origin = new Vector3(0, 1, 0);
    
    public int currentHealth = 5;
    public int level = 1;
}
