using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public enum ItemType { UNKNOWN, SCYTHE, SQUIRREL_SEED, HUMAN_SEED, MYTHOLOGICAL_SEED }

    private static float TIME_BETWEEN_SOULS_EXPIRED = 3f;

    public GameObject squirrelCropPrefab;
    public GameObject humanCropPrefab;
    public GameObject mythologicalCropPrefab;

    private static readonly int MAX_SLOTS = 7;

    // The object that contains LayoutGroup of multiple Inventory Slots
    public GameObject inventorySlotsParent;

    public TMPro.TMP_Text soulCountText;

    private List<InventoryTileController> inventorySlots = new List<InventoryTileController>();
    private int currentItemIndex;

    private Collider2D[] scythedObjects = new Collider2D[1000];
    private Collider2D[] plantableSpaces = new Collider2D[100];
    private int soulCount = 5;

    private float soulExpireTimer;

    // Start is called before the first frame update
    void Start()
    {
        foreach (InventoryTileController inventorySlot in inventorySlotsParent.GetComponentsInChildren<InventoryTileController>()) {
            inventorySlot.Reset();
            inventorySlots.Add(inventorySlot);
        }
        SetCurrentItemIndex(6); // Default to random spot so scythe isn't out at the beginning of the tutorial

        // Create starting inventory content (hacky)
        inventorySlots[1].SetSprite(squirrelCropPrefab.GetComponent<Harvestable>().cropData.uiSeedSprite);
        inventorySlots[1].Add(ItemType.SQUIRREL_SEED);
        inventorySlots[1].Add(ItemType.SQUIRREL_SEED);
        inventorySlots[1].Add(ItemType.SQUIRREL_SEED);

        soulCountText.text = soulCount.ToString();
    }

    internal bool OutOfSouls() {
        if (soulCount > 0) {
            return false;
        }

        foreach (InventoryTileController inventorySlot in inventorySlots) {
            switch (inventorySlot.itemType) {
                case ItemType.UNKNOWN:
                    break;
                case ItemType.SCYTHE:
                    break;
                case ItemType.SQUIRREL_SEED:
                    return false;
                case ItemType.HUMAN_SEED:
                    return false;
                case ItemType.MYTHOLOGICAL_SEED:
                    return false;
            }
        }
        return true;
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
                if (TryPlantSeed(selectedSlot.itemType)) {
                    selectedSlot.Decrement();
                }
                break;
            case ItemType.MYTHOLOGICAL_SEED:
                if (TryPlantSeed(selectedSlot.itemType)) {
                    selectedSlot.Decrement();
                }
                break;
        }
    }

    public void SwingScythe() {
        PlayerController.Instance.ShowSwingScytheAnimation();
        AudioManager.Instance.PlayScytheSwing();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int numObjects = PlayerController.Instance.scytheCollider.OverlapCollider(filter, scythedObjects);

        bool anyHit = false;
        for (int i = 0; i < numObjects; i++) {
            Collider2D scythedObject = scythedObjects[i];
            Harvestable harvestable = scythedObject.GetComponent<Harvestable>();
            if (harvestable && harvestable.CanHarvest()) {
                harvestable.Harvest();
                anyHit = true;
            }
        }

        if (anyHit) {
            AudioManager.Instance.PlayScytheHit();
        }
    }

    public PlantableZone TryFindPlantableZone() {
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        int numObjects = PlayerController.Instance.plantableColliderSpace.OverlapCollider(filter, plantableSpaces);

        PlantableZone closestEmptyZone = null;
        float minDistance = Mathf.Infinity;
        for (int i = 0; i < numObjects; i++) {
            Collider2D plantableZoneCollider = plantableSpaces[i];
            PlantableZone plantableZone = plantableZoneCollider.GetComponent<PlantableZone>();
            if (plantableZone && plantableZone.IsEmpty()) {
                float distance = Vector2.Distance(PlayerController.Instance.plantableColliderSpace.gameObject.transform.position, plantableZone.gameObject.transform.position);
                if (distance < minDistance) {
                    closestEmptyZone = plantableZone;
                    minDistance = distance;
                }
            }
        }
        return closestEmptyZone;
    }

    public bool TryPlantSeed(ItemType itemType) {
        PlantableZone closestEmptyPlantableZone = TryFindPlantableZone();
        Debug.Log(closestEmptyPlantableZone);
        if (closestEmptyPlantableZone == null) {
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
                seedPrefab = humanCropPrefab;
                break;
            case ItemType.MYTHOLOGICAL_SEED:
                seedPrefab = mythologicalCropPrefab;
                break;
        }

        AudioManager.Instance.PlayPlantingSound();
        GameObject spawnedCrop = Instantiate(seedPrefab);
        Harvestable harvestable = spawnedCrop.GetComponent<Harvestable>();
        closestEmptyPlantableZone.Plant(harvestable);
        return true;
    }

    public void AddSoul(int amount) {
        this.soulCount = Mathf.Clamp(this.soulCount + amount, 0, 999);
        soulCountText.text = soulCount.ToString();
    }

    public bool CanSpendSoul(int amount) {
        return this.soulCount >= amount;
    }

    public void SpendSoul(int amount) {
        this.soulCount = Mathf.Clamp(this.soulCount - amount, 0, 999);
        soulCountText.text = soulCount.ToString();
    }

    public void SetCurrentItemIndex(int index) {
        inventorySlots[currentItemIndex].UnhighlightItem();

        if (index < 0) {
            index += MAX_SLOTS;
        }

        if (index >= MAX_SLOTS) {
            index = index % MAX_SLOTS;
        }

        PlayerController.Instance.HideScythe();
        switch (inventorySlots[index].itemType) {
            case ItemType.UNKNOWN:
                break;
            case ItemType.SCYTHE:
                PlayerController.Instance.ShowScythe();
                break;
            case ItemType.SQUIRREL_SEED:
                break;
            case ItemType.HUMAN_SEED:
                break;
        }

        inventorySlots[index].HighlightItem();
        currentItemIndex = index;
    }

    // Update is called once per frame
    void Update()
    {
        // Ignore item selection during the tutorial.
        if (GameManager.Instance.InTutorial()) {
            return;
        }

        soulExpireTimer += Time.deltaTime;
        if (soulExpireTimer > TIME_BETWEEN_SOULS_EXPIRED) {
            soulExpireTimer = 0f;
            SpendSoul(1);
        }

        if (Input.mouseScrollDelta.y > 0f) {
            SetCurrentItemIndex(currentItemIndex - 1);
        } else if (Input.mouseScrollDelta.y < 0f) {
            SetCurrentItemIndex(currentItemIndex + 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            SetCurrentItemIndex(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            SetCurrentItemIndex(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {
            SetCurrentItemIndex(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {
            SetCurrentItemIndex(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {
            SetCurrentItemIndex(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {
            SetCurrentItemIndex(5);
        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {
            SetCurrentItemIndex(6);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {
            SetCurrentItemIndex(7);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {
            SetCurrentItemIndex(8);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            SetCurrentItemIndex(9);
        }

    }
}
