using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text stackText;

    private Item currentItem;
    private float lastClickTime = 0;
    private float doubleClickThreshold = 0.3f;

    private void Awake()
    {
        lastClickTime = 0;
    }

    public void SetItem(Item item)
    {
        currentItem = item;
        icon.sprite = item.data.icon;
     
        if (item is CountableItem countableItem)
        {
            if(countableItem.currentStack > 1)
            {
                stackText.text = countableItem.currentStack.ToString();
                stackText.gameObject.SetActive(true);
            }
            else
            {
                stackText.gameObject.SetActive(false);
            }
        }
        else
        {
            stackText.gameObject.SetActive(false);
        }
    }

    public void Onclick()
    {
        if( currentItem == null)
        {
            Debug.Log("Missing item");
            return;
        }

        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick < doubleClickThreshold)
        {
            Debug.Log("Double Click");
            currentItem.Use();
            lastClickTime = 0;
        }
        else
        {
            lastClickTime = Time.time;
            Debug.Log("Single Click");
        }
    }
}
