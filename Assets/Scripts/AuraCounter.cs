using UnityEngine;
using TMPro;

public class AuraCounter : MonoBehaviour
{
    [SerializeField] private TMP_Text label;

    private int count;

    private void OnEnable()  => TargetHealth.AnyAuraKilledHerself += OnTriggered;
    private void OnDisable() => TargetHealth.AnyAuraKilledHerself -= OnTriggered;

    private void Start() => label.text = $"Score: {count}";

    private void OnTriggered()
    {
        count++;
        label.text = $"Score: {count}";;
    }
}