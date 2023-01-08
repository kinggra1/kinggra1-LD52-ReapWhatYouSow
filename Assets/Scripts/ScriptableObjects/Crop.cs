using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "Farming/Crop", order = 51)]
public class Crop : ScriptableObject {

    public InventoryManager.ItemType type;
    public Sprite uiSeedSprite;

    // Number of souls that need to be spend to receive this Crop seed.
    public int storeCost;
    // How many of the entity are farmed by this crop
    public int spawnCount;

    // How long it takes to go from planted to fully grown.
    public float growingTime;
    // How long until a fully grown crop starts fleeing.
    public float timeToFlee;

    public int numSoulsDropped;

    // Add in a more complex object to determine number/types of souls dropped.
}
