using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crop", menuName = "Farming/Crop", order = 51)]
public class Crop : ScriptableObject {
    public FarmingManager.CropType type;
    // How long it takes to go from planted to fully grown.
    public int growingTime;
    // How long until a fully grown crop starts fleeing.
    public int timeToFlee;
    // How many of the entity are farmed by this crop
    public int spawnCount;

    // Add in a more complex object to determine number/types of souls dropped.
}
