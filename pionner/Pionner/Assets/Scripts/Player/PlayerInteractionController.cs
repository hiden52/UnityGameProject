using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float interactionDistance = 5f;
    [SerializeField] IInteratable currentInteratable;

    private void Update()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 3f, Color.red, 0.1f);
        // �κ��丮UI�� Ȱ��ȭ �Ǿ� ���� ���, ����ĳ��Ʈ ����
        if (!UIManager.Instance.IsInventoryActivated() && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance, layerMask))
        {
            var newInteractable = hit.collider.GetComponent<IInteratable>();

            if (newInteractable != null)
            {
                // InteractionUI Ȱ��ȭ
                UIManager.Instance.ActivateInteractionUI();
                // 
                if (newInteractable != currentInteratable)
                {
                    //Debug.Log("new Interactable found");
                    if(currentInteratable != null)
                    {
                        //Debug.Log("Delete previous event");
                        PlayerInputManager.Instance.OnKeyFPressed -= currentInteratable.Interact;
                    }
                    
                    PlayerInputManager.Instance.OnKeyFPressed += newInteractable.Interact;
                    currentInteratable = newInteractable;
                }
            }
        }
        else
        {
            // ��ȣ�ۿ� ������ ������Ʈ�� ���ٸ� ��ȣ�ۿ� UI�� ��Ȱ��ȭ
            // ��ȣ�ۿ� Ű �̺�Ʈ ���� ���� �� currentInteratable�� null�� �ʱ�ȭ
            if (currentInteratable != null)
            {
                UIManager.Instance.DeactivateInteractionUI();
                PlayerInputManager.Instance.OnKeyFPressed -= currentInteratable.Interact;
                currentInteratable = null;
            }
        }
    }
}
