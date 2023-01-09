using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantableZone : MonoBehaviour
{
    public GameObject dirtPatch;

    private bool isPlanted = false;
    private Harvestable plantedCrop = null;
    // Start is called before the first frame update
    void Start()
    {
        dirtPatch.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (plantedCrop && !plantedCrop.IsGrowing()) {
            plantedCrop = null;
            isPlanted = false;
            dirtPatch.SetActive(false);
        }
        
    }

    internal void Plant(Harvestable harvestable) {
        harvestable.transform.position = this.transform.position;
        this.plantedCrop = harvestable;
        isPlanted = true;
        dirtPatch.SetActive(true);
    }

    internal bool IsEmpty() {
        return !isPlanted;
    }
}
