using SuperMaxim.Messaging;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private string levelUnlockEarthPower;
    [SerializeField] private string levelUnlockIcePower;
    [SerializeField] private string levelUnlockFirePower;

    private List<PowerType> powers = new();

    public void ResetPowers()
    {
        powers.Clear();

        if (SceneController.Instance.IsLevelCompleted(levelUnlockEarthPower))
        {
            AddPower(PowerType.Earth);
        }
        if (SceneController.Instance.IsLevelCompleted(levelUnlockIcePower))
        {
            AddPower(PowerType.Ice);
        }
        if (SceneController.Instance.IsLevelCompleted(levelUnlockFirePower))
        {
            AddPower(PowerType.Fire);
        }
    }

    public void AddPower(PowerType power)
    {
        if (!powers.Contains(power))
        {
            powers.Add(power);
            InitPowers();
        }
    }

    public void InitPowers()
    {
        foreach (var power in powers)
        {
            Messenger.Default.Publish(new LearnedPowerMessage(power));
        }
    }
}
