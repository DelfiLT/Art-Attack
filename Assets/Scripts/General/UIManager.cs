using SuperMaxim.Messaging;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject HUDPanel;

    [SerializeField]
    private GameObject EarthPower;
    [SerializeField]
    private GameObject IcePower;
    [SerializeField]
    private GameObject FirePower;

    [SerializeField]
    private GameObject GameOverPanel;
    [SerializeField]
    private GameObject PausePanel;

    private bool paused;

    private void Start()
    {
        Messenger.Default.Subscribe<PlayerDeathMessage>(HandlePlayerDeat);
        Messenger.Default.Subscribe<LearnedPowerMessage>(HandleLearnedPower);

        GameManager.Instance.InitPowers();
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<PlayerDeathMessage>(HandlePlayerDeat);
        Messenger.Default.Unsubscribe<LearnedPowerMessage>(HandleLearnedPower);
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            Time.timeScale = paused ? 0 : 1;
            PausePanel.SetActive(paused);
        }
    }

    private void HandlePlayerDeat(PlayerDeathMessage message)
    {
        HUDPanel.SetActive(false);
        GameOverPanel.SetActive(true);
    }

    private void HandleLearnedPower(LearnedPowerMessage message)
    {
        switch (message.Type)
        {
            case PowerType.Earth:
                EarthPower.SetActive(true);
                break;
            case PowerType.Ice:
                IcePower.SetActive(true);
                break;
            case PowerType.Fire:
                FirePower.SetActive(true);
                break;
            default:
                break;
        }
    }
}
