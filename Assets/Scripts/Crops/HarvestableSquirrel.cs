using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableSquirrel : Harvestable {
    private static readonly float SOUL_ANIMATION_TIME = 0.5f;
    private static readonly float ARC_MAX_Y = 1f;
    private static readonly float SPEED = 1f;

    public override void Harvest() {
        for (int i = 0; i < 10; i++) {
            GameObject soul = Instantiate(FarmingManager.Instance.soulOnePrefab);
            Collectable collectable = soul.GetComponent<Collectable>();
            soul.transform.position = this.transform.position;
            float randomDelay = Random.Range(0f, 0.1f);
            Vector3 offset = Random.insideUnitCircle;
            Vector3 targetPos = this.transform.position + offset;
            LeanTween.moveX(soul, targetPos.x, SOUL_ANIMATION_TIME);
; ;

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

    // Update is called once per frame
    void Update() {
        // move away from player
        float step = SPEED * Time.deltaTime; // calculate distance to move
        var playerPosition = PlayerController.Instance.transform.position;
        Vector2 directionToPlayer = playerPosition - this.transform.position;
        Vector2 targetDirection = -directionToPlayer;
        Vector3 targetPosition = Vector2.MoveTowards(this.transform.position, (Vector2)this.transform.position + targetDirection, step);
        targetPosition.z = this.transform.position.z;
        this.transform.position = targetPosition;
    }
 }
