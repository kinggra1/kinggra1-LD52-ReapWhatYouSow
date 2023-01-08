using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventoryTileController : MonoBehaviour
{
    private static int MAX_COUNT = 99;

    public InventoryManager.ItemType itemType;
    public bool isStackable = true;

    public GameObject quantityParentObject;
    public TMPro.TMP_Text quantityText;
    public Image uiImage;

    public bool isEmpty = true;
    private int quantity;

    public void Reset() {
        // Keep scythe as is (special hacky case).
        if (itemType != InventoryManager.ItemType.SCYTHE) {
            itemType = InventoryManager.ItemType.UNKNOWN;
            quantity = 0;
            uiImage.sprite = null;
        }
        RefreshGUI();
    }

    public void SetSprite(Sprite sprite) {
        this.uiImage.sprite = sprite;
    }

    public bool CanAdd(InventoryManager.ItemType itemType) {
        return isEmpty || (itemType == this.itemType && isStackable && quantity < MAX_COUNT);
    }

    public void Add(InventoryManager.ItemType itemType) {
        if (isEmpty) {
            this.itemType = itemType;
            isEmpty = false;
        }
        quantity = Mathf.Clamp(quantity + 1, 0, MAX_COUNT);
        RefreshGUI();
    }

    public void Decrement() {
        quantity = Mathf.Clamp(quantity - 1, 0, MAX_COUNT);
        if (quantity == 0) {
            this.itemType = InventoryManager.ItemType.UNKNOWN;
            this.SetSprite(null);
        }
        RefreshGUI();
    }

    private void RefreshGUI() {
        if (!isStackable || quantity == 0) {
            quantityParentObject.SetActive(false);
        } else {
            quantityParentObject.SetActive(true);
            quantityText.text = quantity.ToString();
        }
    }
}
