using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Soul", menuName = "Farming/Soul", order = 51)]
public class Soul : ScriptableObject {
    public GameObject prefab;
    // How many souls is this individual object worth
    public int value;
}
