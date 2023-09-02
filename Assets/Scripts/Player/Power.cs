using FMODUnity;
using SuperMaxim.Messaging;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    public PowerType Type { get { return power; } }

    [SerializeField] private KeyCode key;
    [SerializeField] private PowerType power;
    private PlayerPowerController controller;
    FMOD.Studio.EventInstance ChangePowerSound;

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
                ChangePowerSound = RuntimeManager.CreateInstance("event:/SFX/UI/Level/Power Change/Beep_001");
                ChangePowerSound.start();
                ChangePowerSound.release();
            }
            else
            {
                Unset();
                //se hace esto acá porque si se hace dentro del unset
                //entraría en recursividad infinita, NO INTENTARLO!!!
                controller.SetCurrentPower(null);
            }
        }
    }
}
