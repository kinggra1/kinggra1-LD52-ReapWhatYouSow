using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public enum ItemType { UNKNOWN, SCYTHE, SQUIRREL_SEED, HUMAN_SEED}

    public GameObject squirrelCropPrefab;
    public GameObject humanCropPrefab;

    private static readonly int MAX_SLOTS = 10;

    // The object that contains LayoutGroup of multiple Inventory Slots
    public GameObject inventorySlotsParent;

    public TMPro.TMP_Text soulCountText;

    private List<InventoryTileController> inventorySlots = new List<InventoryTileController>();
    private int currentItemIndex;

    private Collider2D[] scythedObjects = new Collider2D[1000];
    private int soulCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (InventoryTileController inventorySlot in inventorySlotsParent.GetComponentsInChildren<InventoryTileController>()) {
            inventorySlot.Reset();
            inventorySlots.Add(inventorySlot);
        }

        soulCountText.text = soulCount.ToString();
    }

    public bool CanPickUp(Crop cropSeed) {
        foreach (InventoryTileController inventorySlot in inventorySlots) {
            if (inventorySlot.CanAdd(cropSeed.type)) {
                return true;
            }
        }
        return false;
    }

    // Calls to this should be guarded with CanPickUp, it assumes that there will always be an eligible slot.
    public void PickUp(Crop cropSeed) {
        foreach (InventoryTileController inventorySlot in inventorySlots) {
            if (inventorySlot.CanAdd(cropSeed.type)) {
                inventorySlot.Add(cropSeed.type);
                inventorySlot.SetSprite(cropSeed.uiSeedSprite);
                return;
            }
        }
    }

    public void TryUseCurrentItem() {
        InventoryTileController selectedSlot = inventorySlots[currentItemIndex];

        switch (selectedSlot.itemType) {
            case ItemType.UNKNOWN:
                break;
            case ItemType.SCYTHE:
                SwingScythe();
                break;
            case ItemType.SQUIRREL_SEED:
                if (TryPlantSeed(selectedSlot.itemType)) {
                    selectedSlot.Decrement();
                }
                break;
            case ItemType.HUMAN_SEED:
                break;
        }
    }

    public void SwingScythe() {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int numObjects = PlayerController.Instance.scytheCollider.OverlapCollider(filter, scythedObjects);

        for (int i = 0; i < numObjects; i++) {
            Collider2D scythedObject = scythedObjects[i];
            Harvestable harvestable = scythedObject.GetComponent<Harvestable>();
            if (harvestable && harvestable.CanHarvest()) {
                harvestable.Harvest();
            }
        }
    }

    public bool CanPlantSeedHere() {
        return true;
    }

    public bool TryPlantSeed(ItemType itemType) {
        if (!CanPlantSeedHere()) {
            return false;
        }

        GameObject seedPrefab = squirrelCropPrefab;
        switch (itemType) {
            case ItemType.UNKNOWN:
                break;
            case ItemType.SCYTHE:
                break;
            case ItemType.SQUIRREL_SEED:
                seedPrefab = squirrelCropPrefab;
                break;
            case ItemType.HUMAN_SEED:
                break;
        }

        GameObject spawnedCrop = Instantiate(seedPrefab);
        spawnedCrop.transform.position = PlayerController.Instance.transform.position;

        return true;
    }

    public void AddSoul(int amount) {
        this.soulCount = Mathf.Clamp(this.soulCount + amount, 0, 999);
        soulCountText.text = soulCount.ToString();
    }

    public bool CanSpendSoul(int amount) {
        return this.soulCount > amount;
    }

    public void SpendSoul(int amount) {
        this.soulCount = Mathf.Clamp(this.soulCount - amount, 0, 999);
        soulCountText.text = soulCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            currentItemIndex = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            currentItemIndex = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            currentItemIndex = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            currentItemIndex = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            currentItemIndex = 4;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            currentItemIndex = 5;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            currentItemIndex = 6;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            currentItemIndex = 7;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            currentItemIndex = 8;
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            currentItemIndex = 9;
        }

    }
}
