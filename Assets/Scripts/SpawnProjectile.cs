using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class SpawnProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectilePosition;
    [SerializeField] private Transform projectileRotation;

    private InputSystem_Actions actions;

    private void Awake()
    {
        actions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        actions.Player.Attack.performed += OnFire;
        actions.Player.Enable();
    }

    private void OnDisable()
    {
        actions.Player.Attack.performed -= OnFire;
        actions.Player.Disable();
    }

    private void OnFire(InputAction.CallbackContext context)
    {
        if (projectilePrefab == null || projectilePosition == null || projectileRotation == null) return;

        Instantiate(projectilePrefab, projectilePosition.position, projectileRotation.rotation);
    }
}