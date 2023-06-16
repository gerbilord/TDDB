using System.Collections.Generic;
using UnityEngine;

public class Highlighter : MonoBehaviour
{

    private List<Renderer> _renderers;
    [SerializeField]
    private Color color = Color.white;
    
    [SerializeField]
    private float intensity = .2f;

    //helper list to cache all the materials ofd this object
    private List<Material> materials;

    //Gets all the materials from each renderer
    private void Awake()
    {
        // get all the renderers on this object
        _renderers = new List<Renderer>(GetComponentsInChildren<Renderer>());

        materials = new List<Material>();
        foreach (var renderer in _renderers)
        {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            materials.AddRange(new List<Material>(renderer.materials));
        }
    }

    public void ToggleHighlight(bool turnOn, Color highlightColor, float highlightIntensity)
    {
        if (turnOn)
        {
            foreach (var material in materials)
            {
                //We need to enable the EMISSION
                material.EnableKeyword("_EMISSION");
                //before we can set the color

                // Adjust the brightness by multiplying the emission color by 0.5
                Color newColor = highlightColor * highlightIntensity;

                material.SetColor("_EmissionColor", newColor);
            }
        }
        else
        {
            foreach (var material in materials)
            {
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
            }
        }
    }
    
    public void ToggleHighlight(bool val)
    {
        ToggleHighlight(val, color, intensity);
    }
}