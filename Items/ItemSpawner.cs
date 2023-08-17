using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private bool isDebugging;
    [SerializeField]
    private ItemListScriptableObject defaultItemValues;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float spawnDelay;
    [SerializeField]
    private List<ItemInfo> itemOverrideList;

    private MeshRenderer meshRenderer;

    private Dictionary<string, ItemInfo> itemOverrideDictionary;

    private bool isActive;
    private ItemInfo spawnedItem;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        ValidateOverrideItems();
        ReloadItemOverrideDictionary();

        isActive = true;
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed * Time.deltaTime, 0));
    }

    private void ValidateOverrideItems()
    {
        foreach (ItemInfo item in itemOverrideList)
        {
            if (defaultItemValues.items.FindIndex(i => i.itemName == item.itemName) < 0)
            {
                Debug.LogError($"[ItemSpawner] Missing override Item [{item.itemName}] in Defaults_ItemsList", defaultItemValues);
            }
        }
    }

    private void ReloadItemOverrideDictionary()
    {
        itemOverrideDictionary = new Dictionary<string, ItemInfo>();
        foreach (ItemInfo item in itemOverrideList)
        {
            itemOverrideDictionary.Add(item.itemName, item);
        }
    }

    private float CalculateItemTotalWeight(int playerPosition)
    {
        float totalWeightPercentage = 0;
        foreach (ItemInfo item in defaultItemValues.items)
        {
            if (!IsItemBan(item))
            {
                totalWeightPercentage += GetItemSpawnPercentage(item, playerPosition);
            }
        }
        return totalWeightPercentage;
    }

    private IEnumerator SpawnItem()
    {
        yield return new WaitForSeconds(spawnDelay);
        meshRenderer.enabled = true;
        isActive = true;
    }

    private ItemInfo GetRandomItem(int playerPosition)
    {
        float totalWeightPercentage = CalculateItemTotalWeight(playerPosition);
        float randomValue = Random.Range(0, totalWeightPercentage);

        if (isDebugging)
        {

            Debug.Log($"[ItemSpawner] Player positoin: {playerPosition}");
            Debug.Log($"[ItemSpawner] Calculated weight: {totalWeightPercentage}");
            Debug.Log($"[ItemSpawner] Random generated value: {randomValue}");
        }

        float currentRandomValue = randomValue;
        foreach (ItemInfo item in defaultItemValues.items)
        {
            if (!IsItemBan(item))
            {
                float itemSpawnPercentage = GetItemSpawnPercentage(item, playerPosition);

                if (currentRandomValue > itemSpawnPercentage)
                {
                    currentRandomValue -= itemSpawnPercentage;
                    continue;
                }
                Debug.Log(item.itemName);
                return item;
            }
        }
        Debug.LogError($"[ItemSpawner] Error while getting random item, returning the first item in the list\nPlayer positon: {playerPosition}\nCalculated weight: {totalWeightPercentage}\nRandom generated value: {randomValue}");
        return defaultItemValues.items[0];
    }

    private bool IsItemBan(ItemInfo item)
    {
        ItemInfo overrideItem;
        itemOverrideDictionary.TryGetValue(item.itemName, out overrideItem);

        return item.isBan || (overrideItem!= null && overrideItem.isBan);
    }
    
    private float GetItemSpawnPercentage(ItemInfo item, int playerPosition)
    {
        ItemInfo overrideItem;
        itemOverrideDictionary.TryGetValue(item.itemName, out overrideItem);
        float itemSpawnPercentage = overrideItem != null ?
                    playerPosition <= overrideItem.spawnPercentage.Count ?
                        overrideItem.spawnPercentage[playerPosition - 1] : overrideItem.spawnPercentage[overrideItem.spawnPercentage.Count - 1] :
                    playerPosition <= item.spawnPercentage.Count ?
                        item.spawnPercentage[playerPosition - 1] : item.spawnPercentage[item.spawnPercentage.Count - 1];
        
        return itemSpawnPercentage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && isActive)
        {
            isActive = false;
            meshRenderer.enabled = false;

            PlayerController playerController = other.GetComponent<PlayerController>();
            int playerPosition = GameController.Instance.GetKartPosition(playerController.gameObject);
            
            GameObject item = GetRandomItem(playerPosition).itemPrefab;
            if(item.GetComponent<ItemBehaviour>())
                item.GetComponent<ItemBehaviour>().kart = other.gameObject;

            playerController.AddNewItem(item);

            StartCoroutine(SpawnItem());
        }
    }
}
