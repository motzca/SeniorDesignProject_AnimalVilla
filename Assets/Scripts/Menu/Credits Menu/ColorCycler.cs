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
            Color.red,
            Color.yellow,
            Color.green,
            Color.cyan,
            Color.blue,
            Color.magenta,
            Color.red
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
