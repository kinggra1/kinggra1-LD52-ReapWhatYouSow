using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : Singleton<ShopManager>
{
    public Crop squirrelSeedData;
    public Crop humanSeedData;

    public GameObject shopUI;

    private void Start() {
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

    // We've already committed to purchase here, any "CanPackUp" gating should block the UI element itself?
    public void BuySquirrelSeed() {
        InventoryManager.Instance.SpendSoul(squirrelSeedData.storeCost);
        InventoryManager.Instance.PickUp(squirrelSeedData);
    }

    public void BuyHumanSeed() {
        InventoryManager.Instance.SpendSoul(humanSeedData.storeCost);
        InventoryManager.Instance.PickUp(humanSeedData);
    }
}
