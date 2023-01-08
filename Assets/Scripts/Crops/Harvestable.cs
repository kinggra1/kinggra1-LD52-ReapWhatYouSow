using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Harvestable : MonoBehaviour {

    public enum State { GROWING, RIPE, FLEEING }
    private State currentState = State.GROWING;
    public Crop cropData;

    private float stateTimer = 0f;
    private bool isGrown = false;
    private bool isFleeing = false;

    public abstract void Harvest();
    protected abstract void FleeingBehavior();
    protected abstract void RipeBehavior();

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