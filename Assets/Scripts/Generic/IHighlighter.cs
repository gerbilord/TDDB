using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHighlighter
{
    public void ToggleHighlight(bool turnOn, Color highlightColor, float highlightIntensity);
    public void ToggleHighlight(bool val);
}
