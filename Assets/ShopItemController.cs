using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    public GameObject blockingOverlay;
    public Image itemImage;
    public TMP_Text costText;
    public Crop associatedCrop;

    void OnEnable()
    {
        RefreshGUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshGUI() {
        if (InventoryManager.Instance.CanSpendSoul(associatedCrop.storeCost)) {
            blockingOverlay.SetActive(false);
        } else {
            blockingOverlay.SetActive(true);
        }

        costText.text = associatedCrop.storeCost.ToString();
        itemImage.sprite = associatedCrop.uiSeedSprite;
    }
}
