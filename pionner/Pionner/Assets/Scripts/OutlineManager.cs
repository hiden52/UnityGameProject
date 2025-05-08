using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : Singleton<OutlineManager>
{
    [Header("Outline Settings")]
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 0.03f;

    [Tooltip("Custom/Outline 셰이더 할당 필요")]
    [SerializeField] private Shader outlineShader;

    private Material sharedOutlineMaterial; // 공유 아웃라인 머티리얼
    private GameObject currentOutlinedObject = null; // 현재 아웃라인이 적용된 오브젝트
    private GameObject activeOutlineClone = null; // 현재 활성화된 아웃라인 복제본


    protected override void Awake()
    {
        base.Awake();

        if (outlineShader == null)
        {
            Debug.LogError("[OutlineManager] Outline Shader not found! 'Custom/Outline' shader");
            outlineShader = Shader.Find("Custom/Outline"); // 폴백
        }

        if (outlineShader != null)
        {
            sharedOutlineMaterial = new Material(outlineShader);
            sharedOutlineMaterial.color = outlineColor;
            // _sharedOutlineMaterial.SetFloat("_OutlineWidth", outlineWidth); // 셰이더에 _OutlineWidth 프로퍼티가 있다면
        }
        else
        {
            Debug.LogError("[OutlineManager] Outline Shader not found! Disable OutlineManager.");
            enabled = false;
        }
    }

    /// <summary>
    /// 지정된 타겟에 아웃라인을 표시합니다.
    /// </summary>
    public void ShowOutline(GameObject target)
    {
        if (target == null || sharedOutlineMaterial == null) return;

        if (currentOutlinedObject == target && activeOutlineClone != null)
        {
            return;
        }

        // 다른 오브젝트에 아웃라인이 있었다면 먼저 숨김
        if (currentOutlinedObject != null && currentOutlinedObject != target)
        {
            HideOutline();
        }

        currentOutlinedObject = target;

        activeOutlineClone = Instantiate(target, target.transform.position, target.transform.rotation, target.transform);
        activeOutlineClone.name = target.name + " Outline";

        Component[] components = activeOutlineClone.GetComponents<Component>();
        foreach (Component component in components)
        {
            if (!(component is Transform) &&
                !(component is Renderer) &&
                !(component is MeshFilter) &&
                !(component is SkinnedMeshRenderer))
            {
                // Debug.Log($"Destroying component: {component.GetType()} on outline clone");
                Destroy(component);
            }
        }

        // 자식 오브젝트들의 콜라이더도 제거
        Collider[] childColliders = activeOutlineClone.GetComponentsInChildren<Collider>();
        foreach (Collider col in childColliders)
        {
            Destroy(col);
        }

        activeOutlineClone.transform.localScale = Vector3.one * (1f + outlineWidth); // 원본 스케일 기준으로 확장

        Renderer[] renderers = activeOutlineClone.GetComponentsInChildren<Renderer>(); // 자식 포함 모든 렌더러
        if (renderers.Length > 0)
        {
            foreach (Renderer r in renderers)
            {
                r.material = sharedOutlineMaterial; // 공유 머티리얼 사용
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
                // 렌더 큐를 조정하여 아웃라인이 다른 오브젝트에 가려지지 않도록 할 수 있습니다.
                // (셰이더 자체에서 ZWrite Off 등을 설정하는 것이 더 일반적입니다)
                // r.material.renderQueue = 3000; // Transparent 이후, Overlay 이전
            }
        }
        else
        {
            // 루트 오브젝트에만 렌더러가 있는 경우
            Renderer renderer = activeOutlineClone.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = sharedOutlineMaterial;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                // renderer.material.renderQueue = 3000;
            }
        }


        activeOutlineClone.SetActive(true);
    }

    /// <summary>
    /// 현재 적용된 아웃라인을 숨깁니다.
    /// </summary>
    public void HideOutline()
    {
        if (activeOutlineClone != null)
        {
            Destroy(activeOutlineClone);
            activeOutlineClone = null;
        }
        currentOutlinedObject = null;
    }

    private void OnDestroy()
    {
        if (sharedOutlineMaterial != null)
        {
            Destroy(sharedOutlineMaterial);
        }
    }
}


