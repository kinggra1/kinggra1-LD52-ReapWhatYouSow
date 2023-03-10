using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Harvestable : MonoBehaviour {

    protected static readonly float SOUL_ANIMATION_TIME = 0.5f;
    protected static readonly float ARC_MAX_Y = 1f;

    public enum State { GROWING, RIPE, FLEEING }
    private State currentState = State.GROWING;
    public Crop cropData;
    public RuntimeAnimatorController fleeingAnimation;

    private static readonly float SCREEN_MIN_X = -17;
    private static readonly float SCREEN_MAX_X = 17;
    private static readonly float SCREEN_MIN_Y = -11;
    private static readonly float SCREEN_MAX_Y = 11;

    private float stateTimer = 0f;

    private SpriteRenderer spriteRenderer;

    protected abstract void FleeingBehavior();
    protected abstract void RipeBehavior();

    public virtual void Harvest() {
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

    public bool IsGrowing() {
        return currentState == State.GROWING;
    }

    private void Grow() {
        float scaleFactor = Mathf.Lerp(0.1f, cropData.maxGrowthScale, stateTimer / cropData.growingTime);
        this.transform.localScale = Vector3.one * scaleFactor;
    }

    public void Start() {
        this.transform.localScale = Vector3.one * 0.1f;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1; // Lower sorting order while growing so that the crop is behind dirt patch
    }

    public void Update() {
        // Pause growth and movement if tutoral screen is open.
        if (GameManager.Instance.TutorialScreenUp()) {
            return;
        }

        stateTimer += Time.deltaTime;

        switch (currentState) {
            case State.GROWING:
                Grow();
                if (stateTimer > cropData.growingTime) {
                    this.transform.localScale = Vector3.one;
                    spriteRenderer.sortingOrder = 20; // Higher sorting order so we're now on top of dirt patches
                    SetState(State.RIPE);
                }
                break;
            case State.RIPE:
                // pause crop movement if we're in tutorial
                if (GameManager.Instance.InTutorial()) {
                    return;
                }
                RipeBehavior();
                if (stateTimer > cropData.timeToFlee) {
                    SetState(State.FLEEING);
                    if (fleeingAnimation != null) {
                        SetFleeingAnimation();
                    }
                }
                break;
            case State.FLEEING:
                FleeingBehavior();
                break;
        }
    }

    public bool CanHarvest() {
        if (!IsGrowing()) {
            return true;
        }
        return false;
    }

    protected void SetState(State state) {
        stateTimer = 0f;
        this.currentState = state;
    }

    private void SetFleeingAnimation() {
        Animator animator = spriteRenderer.GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = fleeingAnimation;
    }

    protected void RemoveIfOutOfBounds() {
        bool outOfBounds = this.transform.position.x < SCREEN_MIN_X || this.transform.position.x > SCREEN_MAX_X || this.transform.position.y < SCREEN_MIN_Y || this.transform.position.y > SCREEN_MAX_Y;
        if (outOfBounds) {
            Destroy(this.gameObject);
        }
    }
}