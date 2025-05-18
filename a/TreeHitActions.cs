using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeHitActions : MineableObject
{
    [SerializeField]Animator animator;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        Debug.Log("Tree on");
    }
    protected override void PlayHitEffect(Vector3 point)
    {
        base.PlayHitEffect(point);

        Debug.Log("new PlayHitEffect " + this.name);
        animator.SetTrigger("Hit");

    }

    protected override void HarvestComplete()
    {

    }
}
