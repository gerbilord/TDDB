using UnityEngine;

public class OpacityChanger3D : MonoBehaviour, IOpacityChanger
{
    [SerializeField] private float defaultOpacity = 0.5f;
    private Material[][] originalMaterials;
    private Renderer[] renderers;

    private void Awake()
    {
        // Store the original materials on Awake
        renderers = GetComponentsInChildren<Renderer>();
        originalMaterials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer renderer = renderers[i];
            Material[] materials = renderer.materials;
            originalMaterials[i] = new Material[materials.Length];
            for (int j = 0; j < materials.Length; j++)
            {
                originalMaterials[i][j] = materials[j];
            }
        }
    }

    public void ToggleOpacity(bool turnOn, float opacity)
    {
        if (turnOn)
        {
            // Set the opacity on all materials
            foreach (Renderer renderer in renderers)
            {
                Material[] materials = renderer.materials;
                for (int i = 0; i < materials.Length; i++)
                {
                    Material material = materials[i];
                    material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    material.SetInt("_ZWrite", 0);
                    material.DisableKeyword("_ALPHATEST_ON");
                    material.EnableKeyword("_ALPHABLEND_ON");
                    material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

                    Color color = material.color;
                    color.a = opacity;
                    material.color = color;
                }
                renderer.materials = materials;
            }
        }
        else
        {
            ToggleOpacity(true, 1f);
        }
    }

    public void ToggleOpacity(bool val)
    {
        ToggleOpacity(val, defaultOpacity);
    }
}