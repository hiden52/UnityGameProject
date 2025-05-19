using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : Singleton<BuildManager>
{
    [SerializeField] private BuildingRecipeData targetBuildingRecipe;
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private float buildDistance;
    [SerializeField] private LayerMask buildLayer;
    [SerializeField] private bool isBuildMode;
    [SerializeField] private ItemRecipeForwarder recipeForwarder;

    private Camera _camera;
    private Ray ray;
    private bool _canBuild;
    private GameObject _buildingGhost;
    private Renderer[] _ghostRenderers;
    private int _layerMask;
    private int _obstacleLayerMask;
    private readonly int _isEnabledPropertyID = Shader.PropertyToID("_IsEnabled");

    public event Action OnStartBuildMode;


    private void Start()
    {
        _camera = Camera.main;
        _buildingGhost = null;
        isBuildMode = false;
        _canBuild = false;
        _layerMask = 1 << LayerMask.NameToLayer("Land");
        _obstacleLayerMask = ~_layerMask;
        PlayerInputManager.Instance.OnEscapePressed += StopBuildMode;

    }

    private void OnDisable()
    {
        PlayerInputManager.Instance.OnEscapePressed -= StopBuildMode;
    }
    public void StartBuildMode(BuildingRecipeData target)
    {
        PlayerInputManager.Instance.SetCanAttack(false);
        OnStartBuildMode?.Invoke();
        isBuildMode = true;
        targetBuildingRecipe = target;
    }
    public void StopBuildMode()
    {
        PlayerInputManager.Instance.SetCanAttack(true);
        isBuildMode = false;
        targetBuildingRecipe = null;
        DestoryGhost();
    }
    private void Update()
    {
        if (!isBuildMode) return;

        DrawGhost();
        if(Input.GetMouseButtonDown(0))
        {
            PlaceBuilding();
        }
        
    }
    private void DrawGhost()
    {
        if(targetBuildingRecipe == null)
        {
            Debug.LogError($"[BuildManager] Target Building Recipe is null!", targetBuildingRecipe);
        }
        ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hitInfo, buildDistance, _layerMask))
        {
            if (_buildingGhost != null) _buildingGhost.gameObject.SetActive(false);
            return;
        }

        //Debug.DrawRay(_camera.transform.position, _camera.transform.forward * buildDistance, Color.red);
        CreateGhost(hitInfo.point);
        _buildingGhost.transform.position = hitInfo.point;

        _canBuild = CheckPlacementVaildity(hitInfo.point, _buildingGhost.transform.rotation);
        UpdateGhostVisuals();
    }

    private bool CheckPlacementVaildity(Vector3 placementPosition, Quaternion placementRotation)
    {
        if (_buildingGhost == null) return false;
        BoxCollider boxCollider = _buildingGhost.GetComponent<BoxCollider>();
        if( boxCollider == null)
        {
            Debug.LogError($"[BuildManager] Ghost object for '{targetBuildingRecipe.buildingToConstruct.BuildingName}' is missing a BoxCollider!", _buildingGhost);
            return false;
        }

        // 아래 주석들은 흐름과 이유를 잘 이해하고 지울 것.
        //
        // OverlapBox 매개변수 계산:
        // 1. 중심점(Center): 고스트의 월드 위치 + (회전된 로컬 중심 오프셋 * 스케일)
        Vector3 center = placementPosition + placementRotation * Vector3.Scale(boxCollider.center, _buildingGhost.transform.lossyScale);

        // 2. 절반 크기(HalfExtents): 콜라이더 크기의 절반 * 스케일
        // lossyScale은 부모-자식 관계에서 누적된 스케일을 반영합니다. 고스트가 BuildManager의 자식이 아니라면 localScale 사용 가능.
        Vector3 halfExtents = Vector3.Scale(boxCollider.size, _buildingGhost.transform.lossyScale) / 2.0f;

        // 3. 방향(Orientation): 고스트의 현재 회전값
        Quaternion orientation = placementRotation;

        // OverlapBox 실행: 정의된 영역 내에 'obstacleLayerMask'에 해당하는 콜라이더가 있는지 검사
        // QueryTriggerInteraction.UseGlobal 설정 따르거나 필요시 Ignore로 변경
        Collider[] overlaps = Physics.OverlapBox(center, halfExtents, orientation, _obstacleLayerMask, QueryTriggerInteraction.UseGlobal);

        if(overlaps.Length > 0)
        {
            Debug.Log($"Obstacle {overlaps[0]} is found!");
            return false;
        }
        else
        {
            return true;
        }

    }

    private void PlaceBuilding()
    {
        if (!_canBuild) return;

        GameObject building = Instantiate(
            targetBuildingRecipe.buildingToConstruct.prefab
            , _buildingGhost.transform.position
            , _buildingGhost.transform.rotation
            );

        building.GetComponent<CraftBuildingAction>().SetForwarder(recipeForwarder);
        foreach( var item in targetBuildingRecipe.ingredients )
        {
            InventoryManager.Instance.ConsumeItemByData(item.itemData, item.amount);
        }
        StopBuildMode();        
    }

    private void DestoryGhost()
    {
        Destroy(_buildingGhost);
        _buildingGhost = null;
        _ghostRenderers = null;
    }
    private void CreateGhost(Vector3 initPoint)
    {
        if (_buildingGhost != null)
        {
            _buildingGhost.gameObject.SetActive(true);
            return;
        }
        Quaternion targetRotation =_camera.transform.rotation;
        targetRotation.x = 0;
        targetRotation.z = 0;
        targetRotation *= Quaternion.Euler(0, 180, 0);
        

        _buildingGhost = GameObject.Instantiate(
            targetBuildingRecipe.buildingToConstruct.prefab
            , initPoint
            , targetRotation
            , transform
            );
        _buildingGhost.name = $"{targetBuildingRecipe.buildingToConstruct.BuildingName}_Ghost";
        Collider[] colliders = _buildingGhost.GetComponents<Collider>();
        if (colliders.Length >= 0)
        {
            foreach (Collider col in colliders)
            {
                col.enabled = false;
            }
        }

        _ghostRenderers = GetComponentsInChildren<Renderer>();
        SetGhostMaterial();
    }

    private void SetGhostMaterial()
    {
        if (_ghostRenderers != null && ghostMaterial != null)
        {
            foreach (Renderer renderer in _ghostRenderers)
            {
                renderer.sharedMaterial = ghostMaterial;
            }
        }
    }
    private void UpdateGhostVisuals()
    {
        if (_buildingGhost == null) return;

        if (ghostMaterial != null && _ghostRenderers != null)
        {
            MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
            float isEnabledValue = _canBuild ? 1.0f : 0.0f;
            
            foreach (Renderer rend in _ghostRenderers)
            {
                rend.GetPropertyBlock(propBlock);
                propBlock.SetFloat(_isEnabledPropertyID, isEnabledValue);
                rend.SetPropertyBlock(propBlock);
            }

        }
    }

    public ItemRecipeForwarder GetRecipeForwarder()
    {
        if (recipeForwarder != null) return recipeForwarder;
        else return null;
    }
}


