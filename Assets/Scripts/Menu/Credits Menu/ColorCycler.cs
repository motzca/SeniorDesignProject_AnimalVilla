using UnityEngine;
using TMPro;

public class ColorCycler : MonoBehaviour
{
    [Tooltip("Assign a TextMeshPro or TextMeshProUGUI component.")]
    public TMP_Text textComponent;
    public float cycleDuration = 2f;
    private float transitionTime = 0f;
    private Color[] colors;

    void Start()
    {
        InitializeColorSpectrum();
        
        if (!textComponent)
        {
            Debug.LogError("TextComponent is not assigned.", this);
        }
    }

void InitializeColorSpectrum()
{
    colors = new Color[]
    {
        new Color(1f, 0f, 0f), // Red
        new Color(1f, 0.4f, 0f), // Orange
        new Color(1f, 0.8f, 0f), // Yellow
        new Color(0.8f, 1f, 0f), // Lime Green
        new Color(0f, 1f, 0f), // Vivid Green
        new Color(0f, 1f, 1f), // Bright Cyan
        new Color(0.2f, 0.55f, 0.8f), // Bright Blue,
        new Color(0.5f, 0.3f, 0.9f), // Bright Indigo
        new Color(1f, 0f, 1f), // Magenta
        new Color(1f, 0.2f, 0.5f), // Pinkish Red
        new Color(1f, 0f, 0f) // Red (ensures a smooth loop)
    };
}



    void Update()
    {
        if (colors == null || colors.Length < 2)
        {
            Debug.LogWarning("Insufficient colors specified for cycling.");
            return;
        }

        transitionTime += Time.deltaTime;
        float phase = Mathf.Repeat(transitionTime, cycleDuration) / cycleDuration;
        int colorIndex = (int)(phase * (colors.Length - 1));
        float colorLerpFactor = (phase * (colors.Length - 1)) - colorIndex;

        Color currentColor = colors[colorIndex];
        Color nextColor = colors[(colorIndex + 1) % colors.Length];
        Color blendedColor = Color.Lerp(currentColor, nextColor, colorLerpFactor);

        textComponent.color = blendedColor;
    }
}
