using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour {
    protected abstract void CustomBehavior();
    public void Collect() {
        GameObject player = PlayerController.Instance.gameObject;
        Invoke("CustomBehavior", 1f);
    }
}
