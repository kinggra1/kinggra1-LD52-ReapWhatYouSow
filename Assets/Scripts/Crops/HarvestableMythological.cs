using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableMythological : Harvestable {

    private static readonly float SPEED = 2.5f;
    private static readonly float WANDER_TIME = 2f;

    private Vector3 wanderDirection;
    private float wanderTimer = 2;

    public GameObject mythologicalPlantableZone;

    // Update is called once per frame
    protected override void FleeingBehavior() {
        // move away from player
        float step = SPEED * Time.deltaTime; // calculate distance to move
        var playerPosition = PlayerController.Instance.transform.position;
        Vector2 directionToPlayer = playerPosition - this.transform.position;
        Vector2 targetDirection = -directionToPlayer;
        Vector3 targetPosition = Vector2.MoveTowards(this.transform.position, (Vector2)this.transform.position + targetDirection, step);
        targetPosition.z = this.transform.position.z;
        this.transform.position = targetPosition;

        // face direction that phoenix is headed
        Vector3 localScale = this.transform.localScale;
        localScale.x = targetDirection.x < 0 ? 1 : -1;
        this.transform.localScale = localScale;

        // check if crop is offscreen and delete if so
        base.RemoveIfOutOfBounds();
    }

    // RipeBehavior makes human wander until it flees
    // choose a random direction and have it go that way for a set amount of time
    // then switch to a new random direction
    protected override void RipeBehavior() {
        // check to see if it's time to switch directions
        if (wanderTimer >= WANDER_TIME) {
            wanderTimer = 0;
            wanderDirection = Random.insideUnitSphere;
        }

        this.transform.position += wanderDirection * SPEED * Time.deltaTime;
        wanderTimer += Time.deltaTime;

        // face direction that phoenix is headed
        Vector3 localScale = this.transform.localScale;
        localScale.x = wanderDirection.x < 0 ? 1 : -1;
        this.transform.localScale = localScale;
    }

    public override void Harvest() {
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
        base.SetState(State.GROWING);
        this.transform.localScale = Vector3.one * 0.1f;
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sortingOrder = 1; // Lower sorting order while growing so that the crop is behind dirt patch
        Vector3 plantableZonePosition = this.gameObject.transform.position;
        plantableZonePosition.y = plantableZonePosition.y - .25f;
        Instantiate(mythologicalPlantableZone, plantableZonePosition, Quaternion.identity);
    }
}
