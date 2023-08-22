using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class LevelEndTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SceneController.Instance.GoToNexLevel();
        }
    }
}
