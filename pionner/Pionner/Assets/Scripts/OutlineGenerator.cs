using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineGenerator : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private Color outlineColor = Color.yellow;
    [SerializeField] private float outlineWidth = 0.03f;

    private GameObject outlineObj;

    private void Start()
    {
        if (target == null) return;

        outlineObj = Instantiate(target, target.transform.position, target.transform.rotation, target.transform);
        outlineObj.name = target.name + " Outline";

        outlineObj.transform.localScale = Vector3.one + Vector3.one* outlineWidth;

        var renderer = outlineObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material outlineMat = new Material(Shader.Find("Unlit/Color"));
            outlineMat.color = outlineColor;
            renderer.material = outlineMat;

            renderer.material.renderQueue = 2000;
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            renderer.receiveShadows = false;
        }

        Collider col = outlineObj.GetComponent<Collider>();
        if(col != null)
        {
            Destroy(col);
        }

    }
}
