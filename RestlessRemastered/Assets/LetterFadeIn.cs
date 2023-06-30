using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetterFadeIn : MonoBehaviour
{
    public float fadeDuration;
    public float fadeDuration2;
    public float fadeFirst = 2;
    public GameObject[] elements;
    public TextMeshProUGUI[] texts;
    public TextMeshProUGUI[] texts2;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(FadeInLetters());
            StartCoroutine(FadeInLetters2());
        }
    }
    public IEnumerator FadeInLetters()
    {
        foreach(GameObject element in elements)
        {
            element.SetActive(true);
        }
        for (int i = 0; i < texts.Length; i++)
        {
            Color originalColor = texts[i].color;
            Color modifiedColor = originalColor;
            modifiedColor.a = 0f;
            texts[i].color = modifiedColor;
            modifiedColor.a = 0f;
            texts[i].color = modifiedColor;
            if (i > 0)
            {
                yield return new WaitForSeconds(fadeFirst);
                fadeFirst = 0;
                fadeDuration = 0.25f;
            }
            
            for (float t = 0; t < 1f; t += Time.deltaTime / fadeDuration)
            {
                modifiedColor.a = Mathf.Lerp(0f, 1, t);
                texts[i].color =  modifiedColor;
                yield return null;
            }
        }

        StartCoroutine(FadeInLetters2());
    }


    public IEnumerator FadeInLetters2()
    {
        yield return new WaitForSeconds(6);
        for (int i = 0; i < 1; i++)
        {
            Color originalColor = texts2[i].color;
            Color modifiedColor = originalColor;
            modifiedColor.a = 0f;
            texts2[i].color = modifiedColor;
            modifiedColor.a = 0f;
            texts2[i].color = modifiedColor;
            if (i > 0)
            {
                yield return new WaitForSeconds(fadeFirst);
                fadeFirst = 0;
            }

            for (float t = 0; t < 1f; t += Time.deltaTime / fadeDuration)
            {
                modifiedColor.a = Mathf.Lerp(0f, 1, t);
                texts2[i].color = modifiedColor;
                yield return null;
            }
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
