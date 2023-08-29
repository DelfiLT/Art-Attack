using FMODUnity;
using SuperMaxim.Messaging;
using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    [SerializeField] int maxMana = 3;
    [SerializeField] KeyCode key = KeyCode.LeftShift;
    FMOD.Studio.EventInstance ManaOutSound;

    int mana;
    Power currentPower;

    private void Start()
    {
        mana = maxMana;
        Messenger.Default.Publish(new ManaChangeMessage(mana));
    }

    public void SetCurrentPower(Power power)
    {
        if (currentPower != null)
        {
            currentPower.Unset();
        }
        currentPower = power;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key) && mana > 0 && currentPower != null)
        {
            currentPower.Use();
            mana--;
            Messenger.Default.Publish(new ManaChangeMessage(mana));
        }

        if (Input.GetKeyDown(key) && mana == 0)
        {
            ManaOutSound = RuntimeManager.CreateInstance("event:/SFX/UI/Level/Mana/UI_Mana_Out_(Beep_002)");
            ManaOutSound.start();
            ManaOutSound.release();
        }
    }
}
