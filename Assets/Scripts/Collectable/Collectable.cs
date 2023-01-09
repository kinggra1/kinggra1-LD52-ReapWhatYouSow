using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Collectable : MonoBehaviour {

    private static readonly float COLLECT_ANIMATION_TIME = 0.2f;

    public bool active = true;
    private bool triggered = false;
    protected abstract void CustomBehavior();

    private void SetCollectable() {
        active = true;
    }
    public void DelayCollectable(float delay) {
        active = false;
        Invoke("SetCollectable", delay);
    }
    public void Collect() {
        if (triggered) {
            return;
        }

        // Delay collection if these souls have just appeared
        float delay = active ? 0f : 0.5f;
        Invoke("CollectAnimation", delay);
    }

    private void CollectAnimation() {
        triggered = true;
        GameObject player = PlayerController.Instance.gameObject;
        float delay = Random.Range(0f, 0.1f);
        LeanTween.move(this.gameObject, player.transform.position, COLLECT_ANIMATION_TIME).setDelay(delay).setEaseInQuad();
        Invoke("CustomBehavior", COLLECT_ANIMATION_TIME + delay);
    }
}
