using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTowardsMouse : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float rotation = Vector2.SignedAngle(Vector3.up, mousePos - this.transform.position);
        this.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
