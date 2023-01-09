using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static readonly float CAMERA_FREEZE_RADIUS = 2.5f;
    private static readonly float MIN_CAMERA_X = -5f;
    private static readonly float MAX_CAMERA_X = 5f;
    private static readonly float MIN_CAMERA_Y = -5f;
    private static readonly float MAX_CAMERA_Y = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 playerPos = PlayerController.Instance.transform.position + Vector3.down * 1f;

        if (Vector2.Distance(playerPos, this.transform.position) > CAMERA_FREEZE_RADIUS) {
            // Target position is at the edge of the camera freeze radius to avoid jerky lerping
            Vector3 targetPos = playerPos + ((Vector2)this.transform.position - playerPos).normalized * CAMERA_FREEZE_RADIUS;
            
            // Lerp 5% of the way towards the edge of the camera freeze radius any frame we are outside of it
            targetPos = Vector2.Lerp(this.transform.position, targetPos, 0.10f);
            targetPos.x = Mathf.Clamp(targetPos.x, MIN_CAMERA_X, MAX_CAMERA_X);
            targetPos.y = Mathf.Clamp(targetPos.y, MIN_CAMERA_Y, MAX_CAMERA_Y);
            targetPos.z = -10;

            this.transform.position = targetPos;
        }
    }
}
