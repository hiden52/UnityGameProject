using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPlayerSensor : MonoBehaviour
{
    [SerializeField] private DoorActions targetDoorActions;

    private void Start()
    {
        if(targetDoorActions == null )
        {
            targetDoorActions = GetComponentInParent<DoorActions>();
            if (targetDoorActions == null)
            {
                Debug.LogError($"[{gameObject.name}.DoorPlayerSensor] DoorActions is null", this); 
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (targetDoorActions != null)
        {
            targetDoorActions.HandlePlayerEnter(other);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if(targetDoorActions != null)
        {
            targetDoorActions.HandlePlayerStay(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (targetDoorActions != null)
        {
            targetDoorActions.HandlePlayerExit(other);
        }
    }
}
