using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] int itemID;
    [SerializeField] string name;
    [SerializeField] Image itemImg;

    private void Awake()
    {
        itemImg = GetComponent<Image>();
    }




}
