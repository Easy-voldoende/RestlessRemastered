using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemSlots;
    public AudioSource source;
    public int selectedItem;
    public int maxSlots;

    void Start()
    {
        source = GameObject.Find("JumpscareSound").GetComponent<AudioSource>();
    }
    public void PlaySound()
    {
        source.Play();
    }
    void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            selectedItem++;
            if (selectedItem > maxSlots)
            {
                selectedItem = maxSlots;
            }
            SelectItem();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            selectedItem--;
            if (selectedItem <= 0)
            {
                selectedItem = 0;
            }
            SelectItem();
        }
    }
    public void SelectItem()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (i == selectedItem)
            {
                itemSlots[i].SetActive(true);
            }
            else
            {
                itemSlots[i].SetActive(false);
            }
        }
    }
}
