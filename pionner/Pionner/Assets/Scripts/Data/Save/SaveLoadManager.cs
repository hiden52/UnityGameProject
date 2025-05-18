// SaveLoadManager.cs
using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class SaveLoadManager : Singleton<SaveLoadManager>
{

    [SerializeField] private string saveFileName = "game_save.json";

    private string SavePath => Path.Combine(Application.persistentDataPath, saveFileName);

    public void SaveGame()
    {
        try
        {
            SaveData saveData = CreateSaveData();
            string json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(SavePath, json);

            Debug.Log($"게임이 저장되었습니다: {SavePath}");
            ShowSaveNotification();
        }
        catch (Exception e)
        {
            Debug.LogError($"저장 중 오류 발생: {e.Message}");
            ShowErrorNotification("저장 실패");
        }
    }

    public bool LoadGame()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("저장 파일이 없습니다.");
                ShowErrorNotification("저장 파일 없음");
                return false;
            }

            string json = File.ReadAllText(SavePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            ApplySaveData(saveData);
            Debug.Log("게임이 로드되었습니다.");
            ShowLoadNotification();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"로드 중 오류 발생: {e.Message}");
            ShowErrorNotification("로드 실패");
            return false;
        }
    }

    private SaveData CreateSaveData()
    {
        SaveData saveData = new SaveData();

        // 1. 플레이어 데이터 저장
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            saveData.playerPosition = new Vector3Data(player.transform.position);
            saveData.playerRotation = new Vector3Data(player.transform.eulerAngles);
        }

        // 2. 인벤토리 데이터 저장
        if (InventoryManager.Instance != null)
        {
            List<Item> items = InventoryManager.Instance.GetAllItems();
            foreach (Item item in items)
            {
                if (item != null)
                {
                    int amount = 1;
                    if (item is CountableItem countableItem)
                    {
                        amount = countableItem.currentStack;
                    }

                    saveData.inventory.Add(new InventoryItemData(item.Data.itemID, amount));
                }
            }
        }

        // 3. 건물 데이터 저장
        BuildingObject[] buildings = FindObjectsOfType<BuildingObject>();
        foreach (BuildingObject building in buildings)
        {
            if (building.BuildingData != null)
            {
                BuildingSaveData buildingData = new BuildingSaveData(
                    building.BuildingData.BuildingId,
                    building.transform.position,
                    building.transform.eulerAngles,
                    building.gameObject.activeSelf
                );

                // 공장 타입인 경우 생산 데이터 저장
                if (building.BuildingData.buildingType == BuildingType.Factory)
                {
                    //AutomatedCraftSystem craftSystem = building.GetComponent<AutomatedCraftSystem>();
                    //if (craftSystem != null && craftSystem.currentRecipe != null)
                    //{
                    //    buildingData.currentRecipeID = craftSystem.CurrentRecipe.recipeId;
                    //    buildingData.productionProgress = craftSystem.ProductionProgress;
                    //}
                }

                saveData.buildings.Add(buildingData);
            }
        }

        // 4. 게임 시간 저장
        // (게임 내 시간 시스템이 있다면)
        // saveData.gameTime = GameTimeManager.Instance.CurrentTime;

        // 5. 현재 날짜 시간
        saveData.saveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        return saveData;
    }

    private void ApplySaveData(SaveData saveData)
    {
        // 1. 기존 건물 제거 (또는 비활성화)
        BuildingObject[] existingBuildings = FindObjectsOfType<BuildingObject>();
        foreach (BuildingObject building in existingBuildings)
        {
            Destroy(building.gameObject);
        }

        // 2. 플레이어 위치 복원
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = saveData.playerPosition.ToVector3();
            player.transform.eulerAngles = saveData.playerRotation.ToVector3();
        }

        // 3. 인벤토리 복원
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();

            foreach (InventoryItemData itemData in saveData.inventory)
            {
                // 아이템 데이터 매니저로부터 ID로 ItemData 찾기
                ItemData data = FindItemDataByID(itemData.itemID);
                if (data != null)
                {
                    InventoryManager.Instance.AddItem(data, itemData.amount);
                }
            }
        }

        // 4. 건물 복원
        foreach (BuildingSaveData buildingData in saveData.buildings)
        {
            // 건물 데이터 매니저로부터 ID로 BuildingData 찾기
            UnityEngine.Object prefab = FindBuildingPrefabByID(buildingData.buildingID);
            if (prefab != null)
            {
                GameObject buildingObj = Instantiate(
                    prefab,
                    buildingData.position.ToVector3(),
                    Quaternion.Euler(buildingData.rotation.ToVector3())
                ) as GameObject;

                // 활성화 상태 설정
                buildingObj.SetActive(buildingData.isActive);

                // 공장 건물인 경우 생산 데이터 복원
                if (buildingData.currentRecipeID >= 0)
                {
                    //AutomatedCraftSystem craftSystem = buildingObj.GetComponent<AutomatedCraftSystem>();
                    //if (craftSystem != null)
                    //{
                    //    ItemRecipeData recipe = FindRecipeByID(buildingData.currentRecipeID);
                    //    if (recipe != null)
                    //    {
                    //        craftSystem.SetRecipe(recipe);
                    //        craftSystem.SetProductionProgress(buildingData.productionProgress);
                    //    }
                    //}
                }
            }
        }

        // 5. 게임 시간 복원
        // (게임 내 시간 시스템이 있다면)
        // GameTimeManager.Instance.SetTime(saveData.gameTime);
    }

    private ItemData FindItemDataByID(int id)
    {
        ItemData[] allItems = Resources.LoadAll<ItemData>("Data/Items");

        foreach (ItemData item in allItems)
        {
            if (item.itemID == id)
            {
                return item;
            }
        }

        Debug.LogWarning($"아이템 ID {id}를 찾을 수 없습니다.");
        return null;
    }

    private UnityEngine.Object FindBuildingPrefabByID(int id)
    {
        BuildingData[] allBuildings = Resources.LoadAll<BuildingData>("Data/Buildings");

        foreach (BuildingData building in allBuildings)
        {
            if (building.BuildingId == id)
            {
                return building.prefab;
            }
        }

        Debug.LogWarning($"건물 ID {id}를 찾을 수 없습니다.");
        return null;
    }

    // ID로 RecipeData 찾기 (이것은 구현해야 합니다)
    private ItemRecipeData FindRecipeByID(string id)
    {
        // 레시피 데이터 매니저로부터 ID로 레시피 찾기
        // 예: return RecipeDatabase.Instance.GetRecipeByID(id);
        return null; // 실제 구현 필요
    }

    // UI 알림 표시 (이것은 구현해야 합니다)
    private void ShowSaveNotification()
    {
        // 예: UIManager.Instance.ShowNotification("게임이 저장되었습니다.", 2f);
        Debug.Log("게임이 저장되었습니다."); // 임시 구현
    }

    private void ShowLoadNotification()
    {
        // 예: UIManager.Instance.ShowNotification("게임이 로드되었습니다.", 2f);
        Debug.Log("게임이 로드되었습니다."); // 임시 구현
    }

    private void ShowErrorNotification(string message)
    {
        // 예: UIManager.Instance.ShowErrorNotification(message, 3f);
        Debug.LogError(message); // 임시 구현
    }
}