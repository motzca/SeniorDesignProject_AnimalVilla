using System.Collections;
using TMPro;
using UnityEngine;

public class EnhancedTypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent;
    public float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;

    void Start()
    {
        StartTypingEffect();
    }

    public void StartTypingEffect()
    {
        if (textComponent == null)
        {
            Debug.LogError("EnhancedTypewriterEffect: No TMP_Text component assigned.");
            return;
        }

        // Stop the existing coroutine to prevent overlaps if StartTypingEffect is called again.
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        string fullText = textComponent.text;
        textComponent.text = "";
        typingCoroutine = StartCoroutine(TypeTextWithTags(fullText));
    }

    private IEnumerator TypeTextWithTags(string text)
    {
        bool isTag = false;
        bool isClosingTag = false;
        string visibleText = "";

        foreach (char letter in text)
        {
            if (letter == '<')
            {
                isTag = true;
                // Check if it's a closing tag
                isClosingTag = text.Substring(visibleText.Length).StartsWith("</");
            }

            visibleText += letter;

            // Only update text and wait if not in a tag or at the end of a closing tag
            if (!isTag || (isTag && letter == '>' && isClosingTag))
            {
                textComponent.text = visibleText;
                yield return new WaitForSeconds(typingSpeed);
            }

            if (letter == '>')
            {
                isTag = false;
                isClosingTag = false;
            }
        }
    }

    // Optionally add methods to restart or stop the typing effect
    public void RestartTypingEffect()
    {
        StartTypingEffect();
    }

    public void StopTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        textComponent.text = ""; // Or set it to the full text if preferred.
    }
}
