using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public enum ItemType { SCYTHE, SQUIRREL_SEED, HUMAN_SEED}

    // The object that contains LayoutGroup of multiple Inventory Slots
    public GameObject inventorySlotsParent;

    private List<InventoryTileController> inventorySlots = new List<InventoryTileController>();

    // Start is called before the first frame update
    void Start()
    {
        foreach(InventoryTileController inventorySlot in inventorySlotsParent.GetComponentsInChildren<InventoryTileController>()) {
            Debug.Log(inventorySlot);
            inventorySlot.Reset();
            inventorySlots.Add(inventorySlot);
        }
    }

    public bool CanPickUp(ItemType itemType) {
        foreach (InventoryTileController inventorySlot in inventorySlots) {
            if (inventorySlot.CanAdd(itemType)) {
                return true;
            }
        }
        return false;
    }

    // Calls to this should be guarded with CanPickUp, it assumes that there will always be an eligible slot.
    public bool PickUp(ItemType itemType) {
        foreach (InventoryTileController inventorySlot in inventorySlots) {
            if (inventorySlot.CanAdd(itemType)) {
                inventorySlot.CanAdd(itemType);
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
