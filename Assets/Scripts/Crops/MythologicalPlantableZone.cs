using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MythologicalPlantableZone : MonoBehaviour
{
    public float growingTime;
    private float growthTimer = 0f;

    // Update is called once per frame
    void Update()
    {
        growthTimer += Time.deltaTime;

        if (growthTimer > growingTime) {
            Destroy(this.gameObject);
        }
    }
}
