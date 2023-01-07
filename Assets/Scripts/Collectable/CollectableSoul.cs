using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableSoul : Collectable {
    public Soul soul;
    protected override void CustomBehavior() {
        Destroy(this.gameObject);
    }
}
