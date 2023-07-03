using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Sprite[] sprites;
    public float cycleTime = 1.5f;
    private int currentIndex = 0;
    private float timer = 0f;
    public Image spriteRenderer;
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
        spriteRenderer.GetComponent<Animator>().enabled = true;
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

}
