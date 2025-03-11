using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text stackText;

    private Item currentItem;

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
        if( currentItem != null)
        {
            Debug.Log(currentItem.data.itemName + " is Seleted");
        }
    }
}
