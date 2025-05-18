using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftButtonAction : MonoBehaviour
{
    [SerializeField] private ProgressionBarController progressBarController;
    private Button craftButton;

    private void Awake()
    {
        if(progressBarController != null )
        {
            progressBarController.OnCanFillUpdated += SetButtonState;
        }

        craftButton = GetComponentInChildren<Button>();
        craftButton.enabled = false;
    }

    private void SetButtonState(bool canFill)
    {
        if(craftButton != null)
        {
            craftButton.enabled = canFill;
        }
    }
}
