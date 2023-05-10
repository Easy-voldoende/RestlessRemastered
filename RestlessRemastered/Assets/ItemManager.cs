using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public GameObject[] itemSlots;
    public int selectedItem;
    public int maxSlots;

    void Start()
    {
        
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
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            selectedItem--;
            if (selectedItem <= 0)
            {
                selectedItem = 0;
            }
        }
    }
    public void SelectItem()
    {

    }
}
