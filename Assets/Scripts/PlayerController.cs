using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController> {

    private static readonly float MAX_X_MOVE_SPEED = 3f;
    private static readonly float MAX_Y_MOVE_SPEED = 3f;
    private static readonly float MAX_OVERALL_MOVE_SPEED = 3f;
    private static readonly float MAX_IFRAMES = 1f;

    private static readonly float MIN_X_POS = -13.3f;
    private static readonly float MAX_X_POS = 13.3f;
    private static readonly float MIN_Y_POS = -7.3f;
    private static readonly float MAX_Y_POS = 6.6f;

    private static readonly float MAX_AIM_VARIANCE = 35f; // Degrees to either side of aimed point.

    public Collider2D scythCollider;

    private Rigidbody2D rb;

    // Inputs from Update used in FixedUpdate.
    private float xInput;
    private float yInput;
    private bool attackButtonPressed;
    private bool firestormPressed;

    private float currentIFrames = MAX_IFRAMES;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (GameManager.Instance.IsPaused()) {
            return;
        }

        xInput = Input.GetAxis("Horizontal");
        yInput = Input.GetAxis("Vertical");

        if (!attackButtonPressed) {
            attackButtonPressed = Input.GetMouseButtonDown(0);
        }

        if (!firestormPressed) {
            // firestormPressed = Input.GetMouseButton(1);
        }

        if (currentIFrames > 0) {
            currentIFrames -= Time.deltaTime;
        }

        SetFacingBasedOnMouse();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (GameManager.Instance.IsPaused()) {
            return;
        }

        if (attackButtonPressed) {
            attackButtonPressed = false;
        }

        if (firestormPressed) {
            firestormPressed = false;
        }

        Vector3 inputPositionChange = 
            xInput * MAX_X_MOVE_SPEED * Vector3.right 
            + yInput * MAX_Y_MOVE_SPEED * Vector3.up;

        // Clamp combined input + random Unstable movement
        if (inputPositionChange.magnitude > MAX_OVERALL_MOVE_SPEED) {
            inputPositionChange = inputPositionChange.normalized * MAX_OVERALL_MOVE_SPEED;
        }

        // determine new position, then limit it to be in the bounds of the screen
        Vector3 newPosition = transform.position + inputPositionChange * Time.fixedDeltaTime;
        Vector3 clampedNewPosition = new Vector3(Mathf.Clamp(newPosition.x, MIN_X_POS, MAX_X_POS), Mathf.Clamp(newPosition.y, MIN_Y_POS, MAX_Y_POS), 0.0f);

        rb.MovePosition(clampedNewPosition);
    }

    private void SetFacingBasedOnMouse() {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 localScale = this.transform.localScale;
        bool leftOfPlayer = mouseWorldPos.x - this.transform.position.x < 0f;
        localScale.x = Mathf.Abs(localScale.x) * (leftOfPlayer ? -1 : 1);

        this.transform.localScale = localScale;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Collectable collectable = collision.GetComponent<Collectable>();
        if (collectable) {
            collectable.Collect();
        }
    }
}
