using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Sprite[] sprites;
    public Sprite[] sprites2;
    public float cycleTime2;
    public float cycleTime = 1.5f;
    private int currentIndex2 = 0;
    private float timer2 = 0f;
    private int currentIndex = 0;
    private float timer = 0f;
    public Image spriteRenderer;
    public Image spriteRenderer2;
    public Color color;

    private void Start()
    {
        color = new Color(0.7764f, 0, 0, 0);
        color.a = 0;
        if (sprites.Length > 0)
        {
            spriteRenderer.sprite = sprites[currentIndex];
        }
    }

    private void Update()
    {
        Monster();
    }

    public void Monster()
    {
        color.a += 1 * Time.deltaTime;
        spriteRenderer.color =color;
        timer += Time.deltaTime;

        if (timer >= cycleTime)
        {
            timer = 0f;

            currentIndex++;
            if (currentIndex >= sprites.Length)
            {
                currentIndex = 0;
            }

            spriteRenderer.sprite = sprites[currentIndex];
        }
    }

    public void Human()
    {
        timer2 += Time.deltaTime;

        if (timer2 >= cycleTime2)
        {
            timer2 = 0f;

            currentIndex2++;
            if (currentIndex2 >= sprites2.Length)
            {
                currentIndex2 = 0;
            }

            spriteRenderer2.sprite = sprites2[currentIndex2];
        }
    }
}
