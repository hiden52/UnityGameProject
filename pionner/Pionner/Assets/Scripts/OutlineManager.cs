using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : Singleton<OutlineManager>
{
    [SerializeField] private List<GameObject> targets = new List<GameObject>();
    [SerializeField] private List<GameObject> outlines = new List<GameObject>();
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 0.03f;

    private GameObject outlineObj;

    public List<GameObject> Targets => targets;

    protected override void Awake()
    {
        base.Awake();
        outlines.Capacity = 50;
    }

    public void CreateOutlines()
    {
        if (targets.Count <= 0) return;
        foreach (GameObject target in targets)
        {
            DrawOuline(target);
        }
        targets.Clear();
    }
    public void DeleteTargetOutline(GameObject target)
    {
        if (target == null)
        {
            return;
        }
        GameObject outlineGO = target.transform.Find(target.name + " Outline").gameObject;
        if ( outlineGO != null )
        {
            outlines.Remove(outlineGO);
            Destroy(outlineGO);
        }
    }
    public void DeleteOutlines()
    {
        foreach (GameObject outline in outlines)
        {
            Destroy(outline);
        }
        outlines.Clear();
    }

    private void DrawOuline(GameObject target)
    {
        if(target == null) return;

        outlineObj = Instantiate(target, target.transform.position, target.transform.rotation, target.transform);
        outlineObj.name = target.name + " Outline";
        outlines.Add(outlineObj);

        outlineObj.transform.localScale = Vector3.one + Vector3.one * outlineWidth;

        var renderer = outlineObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material outlineMat = new Material(Shader.Find("Custom/Outline"));
            outlineMat.color = outlineColor;
            renderer.material = outlineMat;

            renderer.material.renderQueue = 2000;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        Collider col = outlineObj.GetComponent<Collider>();
        if (col != null)
        {
            Destroy(col);
        }
    }
}


