using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestableHuman : Harvestable {

    private static readonly float SPEED = 1f;

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
    }

    protected override void RipeBehavior() {
        this.transform.position += Vector3.right * (SPEED/2f) * Time.deltaTime;
    }
}
