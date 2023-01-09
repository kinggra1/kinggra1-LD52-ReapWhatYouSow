using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableSquirrel : Harvestable {
    private static readonly float SPEED = 1f;
    private static readonly float WANDER_TIME = 1f;

    private Vector3 wanderDirection;
    private float wanderTimer = 0;

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

        // face direction that squirrel is headed
        Vector3 localScale = this.transform.localScale;
        localScale.x = targetDirection.x < 0 ? 1 : -1;
        this.transform.localScale = localScale;
    }

    // RipeBehavior makes squirrel wander until it flees
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

        // face direction that squirrel is headed
        Vector3 localScale = this.transform.localScale;
        localScale.x = wanderDirection.x < 0 ? 1 : -1;
        this.transform.localScale = localScale;
    }
}
