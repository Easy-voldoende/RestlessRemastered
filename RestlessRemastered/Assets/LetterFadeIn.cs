using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterFadeIn : MonoBehaviour
{
    public float fadeDuration;

    public TextMeshProUGUI[] texts;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(FadeInLetters());
        }
    }
    public IEnumerator FadeInLetters()
    {
        
        for (int i = 0; i < texts.Length; i++)
        {
            Color originalColor = texts[i].color;
            Color modifiedColor = originalColor;
            modifiedColor.a = 0f;
            texts[i].color = modifiedColor;
            float fadeInterval = 0f;
            modifiedColor.a = 0f;
            texts[i].color = modifiedColor;
            yield return new WaitForSeconds(fadeInterval);
            for (float t = 0; t < 1f; t += Time.deltaTime / fadeDuration)
            {
                modifiedColor.a = Mathf.Lerp(0f, 1, t);
                texts[i].color =  modifiedColor;
                yield return null;
            }
        }
    }
}
