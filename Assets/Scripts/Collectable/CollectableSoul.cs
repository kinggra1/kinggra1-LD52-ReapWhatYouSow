using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSoul : Collectable {
    public Soul soul;
    protected override void CustomBehavior() {
        AudioManager.Instance.PlaySoulCollected();
        InventoryManager.Instance.AddSoul(soul.value);
        Destroy(this.gameObject);
    }
}
