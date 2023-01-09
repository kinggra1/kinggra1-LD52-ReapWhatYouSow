using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public GameObject interactionPrompt;

    private bool playerInRange = false;

    // Start is called before the first frame update
    void Start()
    {
        interactionPrompt.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player) {
            player.SetShopInRange(true);
            interactionPrompt.SetActive(true);
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player) {
            player.SetShopInRange(false);
            interactionPrompt.SetActive(false);
            playerInRange = false;
        }
    }
}
