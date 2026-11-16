using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]
struct DisplayExhibit
{
    public Vector3 position;
    public String particleText;
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text particleText;
    [SerializeField] private TMP_Text toggleText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private List<DisplayExhibit> displays;
    
    private DisplayExhibit currentDisplay;
    private int currentIndex = 0;
    private bool toggled = true;
    
    void Start()
    {
        if (displays != null && displays.Count > 0)
        {
            if (cameraTransform != null)
            {
                UpdateInformation();
            }
        }
    }

    public void ToggleInfo()
    {
        toggled = !toggled;
        nextButton.gameObject.SetActive(toggled);
        particleText.gameObject.SetActive(toggled);
        toggleText.text = (toggled) ? "Hide" : "Show";
    }

    public void NextDisplay()
    {
        currentIndex = (currentIndex + 1) % displays.Count;
        UpdateInformation();
    }

    private void UpdateInformation()
    {
        currentDisplay = displays[currentIndex];
        cameraTransform.transform.position = currentDisplay.position;
        particleText.text = currentDisplay.particleText;
    }
}
