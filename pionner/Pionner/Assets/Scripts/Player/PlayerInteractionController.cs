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
        // 인벤토리UI가 활성화 되어 있을 경우, 레이캐스트 무시
        if (!UIManager.Instance.IsInventoryActivated() && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance, layerMask))
        {
            var newInteractable = hit.collider.GetComponent<IInteratable>();

            if (newInteractable != null)
            {
                // InteractionUI 활성화
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
            // 상호작용 가능한 오브젝트가 없다면 상호작용 UI를 비활성화
            // 상호작용 키 이벤트 구독 해제 후 currentInteratable을 null로 초기화
            if (currentInteratable != null)
            {
                UIManager.Instance.DeactivateInteractionUI();
                PlayerInputManager.Instance.OnKeyFPressed -= currentInteratable.Interact;
                currentInteratable = null;
            }
        }
    }
}
