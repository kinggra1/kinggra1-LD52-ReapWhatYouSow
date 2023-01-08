using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public enum ItemType { SCYTHE, SQUIRREL_SEED, HUMAN_SEED}

    // The object that contains LayoutGroup of multiple Inventory Slots
    public GameObject inventorySlotsParent;

    private List<InventoryTileController> inventorySlots = new List<InventoryTileController>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(InventoryTileController inventorySlot in inventorySlotsParent.GetComponentsInChildren<InventoryTileController>()) {
            inventorySlot.Reset();
            inventorySlots.Add(inventorySlot);
        }
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha3)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha4)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha5)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha6)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha7)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha8)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) {

        }
        if (Input.GetKeyDown(KeyCode.Alpha0)) {

        }

    }
}
