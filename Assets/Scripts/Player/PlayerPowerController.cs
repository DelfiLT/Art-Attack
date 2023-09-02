using FMODUnity;
using SuperMaxim.Messaging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    [SerializeField] int maxMana = 3;
    [SerializeField] KeyCode key = KeyCode.LeftShift;
    FMOD.Studio.EventInstance ManaOutSound;

    int mana;
    Power currentPower;

    HashSet<PlatformController> platforms = new();

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
            var affectedPlatforms = new HashSet<PlatformController>();
            Debug.Log("Platforms "+ platforms.Count);
            switch (currentPower.Type)
            {
                case PowerType.Earth:
                    currentPower.Use();
                    break;
                case PowerType.Ice:
                case PowerType.Fire:
                    if (platforms.Count == 0)
                    {
                        Debug.Log("Ninguna plataforma cercana");
                        return;
                    }

                    var platformsArr = platforms.ToArray();
                    foreach (var platform in platformsArr)
                    {
                        if (!platform.AlreadyChanged(currentPower.Type))
                        {
                            platform.ChangeElement(currentPower.Type);
                            affectedPlatforms.Add(platform);
                        }
                    }
                    foreach (var p in affectedPlatforms)
                    {
                        platforms.Remove(p);
                    }
                    break;
                default:
                    break;
            }
            if (affectedPlatforms.Count > 0)
            {
                mana--;
                Messenger.Default.Publish(new ManaChangeMessage(mana));
            }
        }

        if (Input.GetKeyDown(key) && mana == 0)
        {
            ManaOutSound = RuntimeManager.CreateInstance("event:/SFX/UI/Level/Mana/UI_Mana_Out_(Beep_002)");
            ManaOutSound.start();
            ManaOutSound.release();
        }
    }

    public void AddClosePlatform(PlatformController platform)
    {
        platforms.Add(platform);
    }

    public void RemoveClosePlatform(PlatformController platform)
    {
        platforms.Remove(platform);
    }
}
