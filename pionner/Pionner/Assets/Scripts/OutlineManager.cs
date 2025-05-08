using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : Singleton<OutlineManager>
{
    [Header("Outline Settings")]
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private float outlineWidth = 0.03f;

    [Tooltip("Custom/Outline ���̴� �Ҵ� �ʿ�")]
    [SerializeField] private Shader outlineShader;

    private Material sharedOutlineMaterial; // ���� �ƿ����� ��Ƽ����
    private GameObject currentOutlinedObject = null; // ���� �ƿ������� ����� ������Ʈ
    private GameObject activeOutlineClone = null; // ���� Ȱ��ȭ�� �ƿ����� ������


    protected override void Awake()
    {
        base.Awake();

        if (outlineShader == null)
        {
            Debug.LogError("[OutlineManager] Outline Shader not found! 'Custom/Outline' shader");
            outlineShader = Shader.Find("Custom/Outline"); // ����
        }

        if (outlineShader != null)
        {
            sharedOutlineMaterial = new Material(outlineShader);
            sharedOutlineMaterial.color = outlineColor;
            // _sharedOutlineMaterial.SetFloat("_OutlineWidth", outlineWidth); // ���̴��� _OutlineWidth ������Ƽ�� �ִٸ�
        }
        else
        {
            Debug.LogError("[OutlineManager] Outline Shader not found! Disable OutlineManager.");
            enabled = false;
        }
    }

    /// <summary>
    /// ������ Ÿ�ٿ� �ƿ������� ǥ���մϴ�.
    /// </summary>
    public void ShowOutline(GameObject target)
    {
        if (target == null || sharedOutlineMaterial == null) return;

        if (currentOutlinedObject == target && activeOutlineClone != null)
        {
            return;
        }

        // �ٸ� ������Ʈ�� �ƿ������� �־��ٸ� ���� ����
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

        // �ڽ� ������Ʈ���� �ݶ��̴��� ����
        Collider[] childColliders = activeOutlineClone.GetComponentsInChildren<Collider>();
        foreach (Collider col in childColliders)
        {
            Destroy(col);
        }

        activeOutlineClone.transform.localScale = Vector3.one * (1f + outlineWidth); // ���� ������ �������� Ȯ��

        Renderer[] renderers = activeOutlineClone.GetComponentsInChildren<Renderer>(); // �ڽ� ���� ��� ������
        if (renderers.Length > 0)
        {
            foreach (Renderer r in renderers)
            {
                r.material = sharedOutlineMaterial; // ���� ��Ƽ���� ���
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
                // ���� ť�� �����Ͽ� �ƿ������� �ٸ� ������Ʈ�� �������� �ʵ��� �� �� �ֽ��ϴ�.
                // (���̴� ��ü���� ZWrite Off ���� �����ϴ� ���� �� �Ϲ����Դϴ�)
                // r.material.renderQueue = 3000; // Transparent ����, Overlay ����
            }
        }
        else
        {
            // ��Ʈ ������Ʈ���� �������� �ִ� ���
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
    /// ���� ����� �ƿ������� ����ϴ�.
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


