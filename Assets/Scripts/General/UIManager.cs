using SuperMaxim.Messaging;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject HUDPanel;

    [SerializeField]
    private GameObject GameOverPanel;

    private void Start()
    {
        Messenger.Default.Subscribe<PlayerDeathMessage>(HandlePlayerDeat);
    }

    private void HandlePlayerDeat(PlayerDeathMessage message)
    {
        HUDPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }
}
