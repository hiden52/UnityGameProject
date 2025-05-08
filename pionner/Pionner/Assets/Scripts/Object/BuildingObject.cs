using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class BuildingObject : DefaultObject, IInteractable
{
    [SerializeField] private Building building;
    [SerializeField] private List<Animator> animator;
    [SerializeField] bool debug;

    private void Awake()
    {
        debug = false;

        if (building == null)
        {
            building = new Building();
        }

        if (animator != null)
        {
            building.SetAnimator(animator);
        }
    }
    public void Interact()
    {
        // 건물 메뉴UI 오픈
    }

    private void Update()
    {
        if(debug)
        {
            debug = false;
            if (building != null)
            {
                building.SetBlueprint();
            }
        }
    }
}
