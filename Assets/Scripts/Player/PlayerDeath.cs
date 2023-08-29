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
        //Debug.Log($"DEATH TRIGGER WITH {collision.gameObject.name}");
        if (collision.CompareTag("Water"))
        {
            Die();
        }

        if(collision.CompareTag("Ice") && collision.bounds.Contains(transform.position))
        {
            Die();
        }
    }

    private void Die()
    {
        playerController.enabled = false;
        Messenger.Default.Publish(new PlayerDeathMessage());
    }
}
