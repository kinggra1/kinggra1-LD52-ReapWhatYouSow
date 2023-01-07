using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour {
    public float seconds = 1f;
    // Start is called before the first frame update
    void Start() {
        Destroy(gameObject, seconds);
    }

    // Update is called once per frame
    void Update() {
        
    }
}
