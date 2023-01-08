using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Harvestable : MonoBehaviour {

    private static readonly float SOUL_ANIMATION_TIME = 0.5f;
    private static readonly float ARC_MAX_Y = 1f;

    public enum State { GROWING, RIPE, FLEEING }
    private State currentState = State.GROWING;
    public Crop cropData;

    private float stateTimer = 0f;
    private bool isGrown = false;
    private bool isFleeing = false;

    protected abstract void FleeingBehavior();
    protected abstract void RipeBehavior();

    public void Harvest() {
        for (int i = 0; i < cropData.numSoulsDropped; i++) {
            GameObject soul = Instantiate(FarmingManager.Instance.soulOnePrefab);
            Collectable collectable = soul.GetComponent<Collectable>();
            soul.transform.position = this.transform.position;
            float randomDelay = Random.Range(0f, 0.1f);
            Vector3 offset = Random.insideUnitCircle;
            Vector3 targetPos = this.transform.position + offset;
            LeanTween.moveX(soul, targetPos.x, SOUL_ANIMATION_TIME);

            LeanTween.moveY(soul, targetPos.y + ARC_MAX_Y, SOUL_ANIMATION_TIME / 2f)
                .setDelay(randomDelay)
                .setEaseOutCubic()
                .setOnComplete(
                    () => LeanTween.moveY(soul, targetPos.y, SOUL_ANIMATION_TIME / 2f)
                    .setEaseInCubic());

            collectable.DelayCollectable(SOUL_ANIMATION_TIME);
        }
        Destroy(this.gameObject);
    }

    private void Grow() {
        float scaleFactor = Mathf.Lerp(0.1f, 1f, stateTimer / cropData.growingTime);
        this.transform.localScale = Vector3.one * scaleFactor;
    }

    public void Start() {
        this.transform.localScale = Vector3.one * 0.1f;
    }

    public void Update() {
        stateTimer += Time.deltaTime;

        switch (currentState) {
            case State.GROWING:
                Grow();
                if (stateTimer > cropData.growingTime) {
                    this.transform.localScale = Vector3.one;
                    SetState(State.RIPE);
                }
                break;
            case State.RIPE:
                RipeBehavior();
                if (stateTimer > cropData.timeToFlee) {
                    SetState(State.FLEEING);
                }
                break;
            case State.FLEEING:
                FleeingBehavior();
                break;
        }
    }

    private void SetState(State state) {
        stateTimer = 0f;
        this.currentState = state;
    }
}