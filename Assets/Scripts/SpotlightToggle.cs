using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Light))]
public class SpotlightToggle : MonoBehaviour
{
    [SerializeField] private bool startOn = true;

    private Light flashlight;
    private InputAction toggleAction;

    void Awake()
    {
        flashlight = GetComponent<Light>();
        flashlight.enabled = startOn;
    }

    private void OnEnable()
    {
        toggleAction = InputSystem.actions.FindAction("Crouch");

        if (toggleAction == null)
        {
            Debug.LogWarning($"FlashlightToggle: no input action  found.", this);
            return;
        }

        toggleAction.performed += OnTogglePerformed;
    }

    private void OnDisable()
    {
        if (toggleAction != null)
            toggleAction.performed -= OnTogglePerformed;
    }

    private void OnTogglePerformed(InputAction.CallbackContext ctx)
    {
        flashlight.enabled = !flashlight.enabled;
    }
}