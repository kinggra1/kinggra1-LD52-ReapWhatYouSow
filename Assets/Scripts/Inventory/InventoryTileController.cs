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

    public TMPro.TMP_Text quantityText;
    public Image uiImage;

    private bool isEmpty = true;
    private int quantity;

    public void Reset() {
        quantity = 0;
        uiImage.sprite = null;
        RefreshGUI();
    }

    public void SetSprite(Sprite sprite) {
        this.uiImage.sprite = sprite;
    }

    public bool CanAdd(InventoryManager.ItemType itemType) {
        return isEmpty || (itemType == this.itemType && quantity < MAX_COUNT);
    }

    public void Add(InventoryManager.ItemType itemType) {
        if (isEmpty) {
            this.itemType = itemType;
        }
        quantity = Mathf.Clamp(quantity + 1, 0, MAX_COUNT);
        RefreshGUI();
    }

    private void RefreshGUI() {
        quantityText.text = quantity.ToString();
    }
}