using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CropAI : MonoBehaviour {

    private bool triggered = false;
    protected abstract void CustomBehavior();
    public void Collect() {
        if (triggered) {
            return;
        }

        triggered = true;
        GameObject player = PlayerController.Instance.gameObject;
        LeanTween.move(this.gameObject, player.transform.position, 0.2f).setEaseInQuad();
        Invoke("CustomBehavior", 0.2f);
    }
}
