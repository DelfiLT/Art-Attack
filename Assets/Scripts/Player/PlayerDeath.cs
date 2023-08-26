using SuperMaxim.Messaging;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private TarodevController.PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<TarodevController.PlayerController>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Water"))
        {
            playerController.enabled = false;
            Messenger.Default.Publish(new PlayerDeathMessage());
        }
    }
}
