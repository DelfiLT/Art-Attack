using SuperMaxim.Messaging;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] private KeyCode key;
    [SerializeField] private PowerType power;
    private PlayerPowerController controller;

    private bool isEnabled;

    private void Start()
    {
        controller = GetComponent<PlayerPowerController>();
    }

    public void Set()
    {
        controller.SetCurrentPower(this);
        Messenger.Default.Publish(new PowerEnableMessage(power, true));
        isEnabled = true;
    }

    public void Unset()
    {
        Messenger.Default.Publish(new PowerEnableMessage(power, false));
        isEnabled = false;
    }

    public abstract void Use();

    protected void Update()
    {
        if (Input.GetKeyDown(key))
        {
            if (!isEnabled)
            {
                Set();
            }
            else
            {
                Unset();
            }
        }
    }
}
