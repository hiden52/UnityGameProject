using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Building
{
    [SerializeField] private BuildingData data;
    [SerializeField] private List<Animator> animators = new List<Animator>();

    [SerializeField] public bool isBluePrint;

    public void SetBlueprint()
    {
        StopAnimations();
    }

    private void StopAnimations()
    {
        if (animators != null)
        {
            foreach (var anim in animators)
            {
                anim.enabled = false;
            }
        }
    }
    public void SetAnimator(List<Animator> animators)
    {
        foreach(var anim in animators)
        {
            this.animators.Add(anim);
        }
    }


    
   





}
