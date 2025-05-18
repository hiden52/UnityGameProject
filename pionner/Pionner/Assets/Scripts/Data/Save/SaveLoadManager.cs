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

        // 1. �÷��̾� ������ ����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            saveData.playerPosition = new Vector3Data(player.transform.position);
            saveData.playerRotation = new Vector3Data(player.transform.eulerAngles);
        }

        // 2. �κ��丮 ������ ����
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

        // 3. �ǹ� ������ ����
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

                // ���� Ÿ���� ��� ���� ������ ����
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

        // 4. ���� �ð� ����
        // (���� �� �ð� �ý����� �ִٸ�)
        // saveData.gameTime = GameTimeManager.Instance.CurrentTime;

        // 5. ���� ��¥ �ð�
        saveData.saveDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        return saveData;
    }

    private void ApplySaveData(SaveData saveData)
    {
        // 1. ���� �ǹ� ���� (�Ǵ� ��Ȱ��ȭ)
        BuildingObject[] existingBuildings = FindObjectsOfType<BuildingObject>();
        foreach (BuildingObject building in existingBuildings)
        {
            Destroy(building.gameObject);
        }

        // 2. �÷��̾� ��ġ ����
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = saveData.playerPosition.ToVector3();
            player.transform.eulerAngles = saveData.playerRotation.ToVector3();
        }

        // 3. �κ��丮 ����
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.ClearInventory();

            foreach (InventoryItemData itemData in saveData.inventory)
            {
                // ������ ������ �Ŵ����κ��� ID�� ItemData ã��
                ItemData data = FindItemDataByID(itemData.itemID);
                if (data != null)
                {
                    InventoryManager.Instance.AddItem(data, itemData.amount);
                }
            }
        }

        // 4. �ǹ� ����
        foreach (BuildingSaveData buildingData in saveData.buildings)
        {
            // �ǹ� ������ �Ŵ����κ��� ID�� BuildingData ã��
            UnityEngine.Object prefab = FindBuildingPrefabByID(buildingData.buildingID);
            if (prefab != null)
            {
                GameObject buildingObj = Instantiate(
                    prefab,
                    buildingData.position.ToVector3(),
                    Quaternion.Euler(buildingData.rotation.ToVector3())
                ) as GameObject;

                // Ȱ��ȭ ���� ����
                buildingObj.SetActive(buildingData.isActive);

                // ���� �ǹ��� ��� ���� ������ ����
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

        // 5. ���� �ð� ����
        // (���� �� �ð� �ý����� �ִٸ�)
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

    // ID�� RecipeData ã�� (�̰��� �����ؾ� �մϴ�)
    private ItemRecipeData FindRecipeByID(string id)
    {
        // ������ ������ �Ŵ����κ��� ID�� ������ ã��
        // ��: return RecipeDatabase.Instance.GetRecipeByID(id);
        return null; // ���� ���� �ʿ�
    }

    // UI �˸� ǥ�� (�̰��� �����ؾ� �մϴ�)
    private void ShowSaveNotification()
    {
        // ��: UIManager.Instance.ShowNotification("������ ����Ǿ����ϴ�.", 2f);
        Debug.Log("������ ����Ǿ����ϴ�."); // �ӽ� ����
    }

    private void ShowLoadNotification()
    {
        // ��: UIManager.Instance.ShowNotification("������ �ε�Ǿ����ϴ�.", 2f);
        Debug.Log("������ �ε�Ǿ����ϴ�."); // �ӽ� ����
    }

    private void ShowErrorNotification(string message)
    {
        // ��: UIManager.Instance.ShowErrorNotification(message, 3f);
        Debug.LogError(message); // �ӽ� ����
    }
}