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

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            saveData.playerPosition = new Vector3Data(player.transform.position);
            saveData.playerRotation = new Vector3Data(player.transform.eulerAngles);
            Debug.Log("Saved Position " + player.name + " "+ saveData.playerPosition.ToVector3(), player);
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다!");
        }

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

        // saveData.gameTime = GameManager.Instance.CurrentTime;

        saveData.saveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        return saveData;
    }

    private void ApplySaveData(SaveData saveData)
    {
        BuildingObject[] existingBuildings = FindObjectsOfType<BuildingObject>();
        foreach (BuildingObject building in existingBuildings)
        {
            Destroy(building.gameObject);
        }

        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            Debug.Log($"로드 위치: {player.name} 현재 위치: {player.transform.position} → 로드 위치: {saveData.playerPosition.ToVector3()}", player);
            
            Vector3 loadPosition = saveData.playerPosition.ToVector3();
            if (loadPosition.y < -10 || loadPosition.y > 100)
            {
                Debug.LogWarning("로드 위치가 비정상적입니다. 기본 위치로 설정합니다.");
                loadPosition = new Vector3(0, 1, 0); // 안전한 기본 위치
            }

            player.transform.position = saveData.playerPosition.ToVector3();
            player.transform.eulerAngles = saveData.playerRotation.ToVector3();
        }
        else
        {
            Debug.LogError("Player 오브젝트를 찾을 수 없습니다!");
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();

            foreach (InventoryItemData itemData in saveData.inventory)
            {
                ItemData data = FindItemDataByID(itemData.itemID);
                if (data != null)
                {
                    InventoryManager.Instance.AddItem(data, itemData.amount);
                }
            }
        }

        foreach (BuildingSaveData buildingData in saveData.buildings)
        {
            UnityEngine.Object prefab = FindBuildingPrefabByID(buildingData.buildingID);
            if (prefab != null)
            {
                GameObject buildingObj = Instantiate(
                    prefab,
                    buildingData.position.ToVector3(),
                    Quaternion.Euler(buildingData.rotation.ToVector3())
                ) as GameObject;

                buildingObj.SetActive(buildingData.isActive);

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


        // GameManager.Instance.SetTime(saveData.gameTime);
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


    // UI 알림 표시 (이것은 구현해야 합니다)
    private void ShowSaveNotification()
    {
        if(AlertManager.Instance != null)
        {
            AlertManager.Instance.ShowSaveAlert();
        }
        Debug.Log("게임이 저장되었습니다."); // 임시 구현
    }

    private void ShowLoadNotification()
    {
        if (AlertManager.Instance != null)
        {
            AlertManager.Instance.ShowLoadAlert();
        }
        Debug.Log("게임이 로드되었습니다."); // 임시 구현
    }

    private void ShowErrorNotification(string message)
    {
        if (AlertManager.Instance != null)
        {
            AlertManager.Instance.ShowErrorAlert(message);
        }
        Debug.LogError(message); // 임시 구현
    }
}