using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantableZone : MonoBehaviour
{
    private bool isPlanted = false;
    private Harvestable plantedCrop = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (plantedCrop && !plantedCrop.IsGrowing()) {
            plantedCrop = null;
            isPlanted = false;
        }
        
    }

    internal void Plant(Harvestable harvestable) {
        harvestable.transform.position = this.transform.position;
        this.plantedCrop = harvestable;
        isPlanted = true;
    }

    internal bool IsEmpty() {
        return !isPlanted;
    }
}
