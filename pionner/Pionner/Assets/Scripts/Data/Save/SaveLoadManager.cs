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

            Debug.Log($"������ ����Ǿ����ϴ�: {SavePath}");
            ShowSaveNotification();
        }
        catch (Exception e)
        {
            Debug.LogError($"���� �� ���� �߻�: {e.Message}");
            ShowErrorNotification("���� ����");
        }
    }

    public bool LoadGame()
    {
        try
        {
            if (!File.Exists(SavePath))
            {
                Debug.LogWarning("���� ������ �����ϴ�.");
                ShowErrorNotification("���� ���� ����");
                return false;
            }

            string json = File.ReadAllText(SavePath);
            SaveData saveData = JsonUtility.FromJson<SaveData>(json);

            ApplySaveData(saveData);
            Debug.Log("������ �ε�Ǿ����ϴ�.");
            ShowLoadNotification();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"�ε� �� ���� �߻�: {e.Message}");
            ShowErrorNotification("�ε� ����");
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
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�!");
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
            Debug.Log($"�ε� ��ġ: {player.name} ���� ��ġ: {player.transform.position} �� �ε� ��ġ: {saveData.playerPosition.ToVector3()}", player);
            
            Vector3 loadPosition = saveData.playerPosition.ToVector3();
            if (loadPosition.y < -10 || loadPosition.y > 100)
            {
                Debug.LogWarning("�ε� ��ġ�� ���������Դϴ�. �⺻ ��ġ�� �����մϴ�.");
                loadPosition = new Vector3(0, 1, 0); // ������ �⺻ ��ġ
            }

            player.transform.position = saveData.playerPosition.ToVector3();
            player.transform.eulerAngles = saveData.playerRotation.ToVector3();
        }
        else
        {
            Debug.LogError("Player ������Ʈ�� ã�� �� �����ϴ�!");
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

        Debug.LogWarning($"������ ID {id}�� ã�� �� �����ϴ�.");
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

        Debug.LogWarning($"�ǹ� ID {id}�� ã�� �� �����ϴ�.");
        return null;
    }


    // UI �˸� ǥ�� (�̰��� �����ؾ� �մϴ�)
    private void ShowSaveNotification()
    {
        if(AlertManager.Instance != null)
        {
            AlertManager.Instance.ShowSaveAlert();
        }
        Debug.Log("������ ����Ǿ����ϴ�."); // �ӽ� ����
    }

    private void ShowLoadNotification()
    {
        if (AlertManager.Instance != null)
        {
            AlertManager.Instance.ShowLoadAlert();
        }
        Debug.Log("������ �ε�Ǿ����ϴ�."); // �ӽ� ����
    }

    private void ShowErrorNotification(string message)
    {
        if (AlertManager.Instance != null)
        {
            AlertManager.Instance.ShowErrorAlert(message);
        }
        Debug.LogError(message); // �ӽ� ����
    }
}