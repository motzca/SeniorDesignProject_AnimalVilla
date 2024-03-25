using System.Collections;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    public TMP_Text textComponent;
    public float typingSpeed = 0.05f;
    private Coroutine typingCoroutine;
    private string fullText; 

    void Awake()
    {
        fullText = textComponent.text;
        textComponent.text = "";
    }

    void OnEnable()
    {
        StartTypingEffect();
    }

    public void StartTypingEffect()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
        textComponent.text = "";
        typingCoroutine = StartCoroutine(DelayStart(fullText, .5f));
    }

    private IEnumerator DelayStart(string text, float delay)
    {
        yield return new WaitForSeconds(delay);
        typingCoroutine = StartCoroutine(TypeTextWithTags(text));
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
                isClosingTag = text.Substring(visibleText.Length).StartsWith("</");
            }

            visibleText += letter;

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
        textComponent.text = ""; 
    }
}
