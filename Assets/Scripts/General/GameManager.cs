using SuperMaxim.Messaging;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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

    private List<PowerType> powers = new();

    void Start() {}

    void Update() {}

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
