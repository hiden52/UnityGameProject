using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Fn : MonoBehaviour
{
    public Button btn;
    public Text text;
    public string str;

    protected virtual void Awake()
    {
        btn = GetComponent<Button>();
        text = GetComponentInChildren<Text>();
    }

    private void Start()
    {
        btn.onClick.AddListener(OnClick);
        SetText(str);
    }

    protected virtual void OnClick()
    {
    }

    protected void SetText(string str)
    {
        text.text = str;
    }

}
