using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float interactionDistance = 5f;
    private bool resetInteractEvnet = false;

    private void Update()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3f, Color.red, 0.1f);
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance, layerMask))
        {
            UIManager.Instance.SetStateInteractUI(true);
            PlayerInputManager.Instance.OnKeyFPressed -= hit.collider.GetComponent<IInteratable>().Interact;
            PlayerInputManager.Instance.OnKeyFPressed += hit.collider.GetComponent<IInteratable>().Interact;
            resetInteractEvnet = true;
        }
        else
        {
            UIManager.Instance.SetStateInteractUI(false);
            if (resetInteractEvnet)
            {
                resetInteractEvnet = false;
                PlayerInputManager.Instance.ClearFKeyEvent();
            }
        }
    }
}
