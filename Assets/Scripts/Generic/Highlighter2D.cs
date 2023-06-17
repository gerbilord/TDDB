using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Highlighter2D : MonoBehaviour, IHighlighter
{

    // get reference to child object called overlay
    
    RawImage overlay;
    [SerializeField]
    private Color color = Color.white;
    
    [SerializeField]
    private float intensity = .2f;

    private void Awake()
    {
        // get component raw image from child
        overlay = GetComponentInChildren<RawImage>();
    }

    public void ToggleHighlight(bool turnOn, Color highlightColor, float highlightIntensity)
    {
        if (turnOn)
        {
            // enable overlay
            overlay.enabled = true;
            // set color
            overlay.color = highlightColor * highlightIntensity;
        }
        else
        {
            // disable overlay
            overlay.enabled = false;
        }
    }
    
    public void ToggleHighlight(bool val)
    {
        ToggleHighlight(val, color, intensity);
    }
}