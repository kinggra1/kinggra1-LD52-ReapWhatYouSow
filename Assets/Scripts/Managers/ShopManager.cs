using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    public Crop squirrelSeedData;
    public Crop humanSeedData;

    public GameObject shopUI;

    private List<ShopItemController> shopItems = new List<ShopItemController>();

    private void Start() {
        foreach (ShopItemController shopItem in shopUI.GetComponentsInChildren<ShopItemController>()) {
            shopItem.RefreshGUI();
            shopItems.Add(shopItem);
        }
        shopUI.SetActive(false);
    }

    public void OpenShop() {
        GameManager.Instance.Pause();
        shopUI.SetActive(true);
    }

    public void CloseShop() {
        GameManager.Instance.Play();
        shopUI.SetActive(false);
    }

    public void RefreshShopUI() {
        foreach (ShopItemController shopItem in shopItems) {
            shopItem.RefreshGUI();
        }
    }

    public void BuySeed(Crop seedData) {
        InventoryManager.Instance.SpendSoul(seedData.storeCost);
        InventoryManager.Instance.PickUp(seedData);
        RefreshShopUI();
    }

    // We've already committed to purchase here, any "CanPackUp" gating should block the UI element itself?
    public void BuySquirrelSeed() {
        InventoryManager.Instance.SpendSoul(squirrelSeedData.storeCost);
        InventoryManager.Instance.PickUp(squirrelSeedData);
        RefreshShopUI();
    }

    public void BuyHumanSeed() {
        InventoryManager.Instance.SpendSoul(humanSeedData.storeCost);
        InventoryManager.Instance.PickUp(humanSeedData);
        RefreshShopUI();
    }
}
