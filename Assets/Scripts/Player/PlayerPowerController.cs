using FMODUnity;
using SuperMaxim.Messaging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    [SerializeField] int maxMana = 3;
    [SerializeField] KeyCode key = KeyCode.LeftShift;
    
    //instancia fmod
    FMOD.Studio.EventInstance ManaOutSound;
    FMOD.Studio.EventInstance EarthPowerSound;
    FMOD.Studio.EventInstance FirePowerSound;
    FMOD.Studio.EventInstance IcePowerSound;

    [SerializeField] private SpriteRenderer earthPlaceholder;
    [SerializeField] private GameObject iceAura;
    [SerializeField] private GameObject fireAura;

    int mana;
    Power currentPower;

    HashSet<PlatformController> platforms = new();

    private void Start()
    {
        mana = maxMana;
        Messenger.Default.Publish(new ManaChangeMessage(mana));
        Messenger.Default.Subscribe<LearnedPowerMessage>(EnablePower);

        GameManager.Instance.ResetPowers();
        GameManager.Instance.InitPowers();
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<LearnedPowerMessage>(EnablePower);
    }

    private void EnablePower(LearnedPowerMessage message)
    {
        switch (message.Type)
        {
            case PowerType.Earth:
                GetComponent<EarthPower>().enabled = true;
                break;
            case PowerType.Ice:
                GetComponent<IcePower>().enabled = true;
                break;
            case PowerType.Fire:
                GetComponent<FirePower>().enabled = true;
                break;
            default:
                break;
        }
    }

    public void SetCurrentPower(Power power)
    {
        if (currentPower != null)
        {
            currentPower.Unset();
            earthPlaceholder.enabled = false;
            iceAura.SetActive(false);
            fireAura.SetActive(false);
        }
        else
        {
            SetAura(power.Type);
        }

        currentPower = power;
    }

    private void SetAura(PowerType power)
    {
        switch (power)
        {
            case PowerType.Earth:
                earthPlaceholder.enabled = true;
                iceAura.SetActive(false);
                fireAura.SetActive(false);
                break;
            case PowerType.Ice:
                earthPlaceholder.enabled = false;
                iceAura.SetActive(true);
                fireAura.SetActive(false);
                break;
            case PowerType.Fire:
                earthPlaceholder.enabled = false;
                iceAura.SetActive(false);
                fireAura.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(key) && mana > 0 && currentPower != null)
        {

            switch (currentPower.Type)
            {
                case PowerType.Earth:
                    currentPower.Use();
                    ConsumeMana();
                    EarthPowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Earth_Use_(FX_015)");
                    EarthPowerSound.start();
                    EarthPowerSound.release();
                    break;
                case PowerType.Ice:
                case PowerType.Fire:
                    if (platforms.Count == 0)
                    {
                        Debug.Log("Ninguna plataforma cercana");
                        return;
                    }

                    var affectedPlatforms = new HashSet<PlatformController>();

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

                    if (affectedPlatforms.Count > 0)
                    {
                        ConsumeMana();
                        if (currentPower.Type == PowerType.Fire) 
                        {
                            FirePowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Fire_Use");
                            FirePowerSound.start();
                            FirePowerSound.release();
                        }
                        else 
                        {
                            IcePowerSound = RuntimeManager.CreateInstance("event:/SFX/Player/Powers/UI_Power_Ice_Use_(FX_036)");
                            IcePowerSound.start();
                            IcePowerSound.release();
                        }
                    }
                    break;
                default:
                    break;
            }

            SetCurrentPower(null);
        }

        //Sonido cuando te quedas sin maná
        if (Input.GetKeyDown(key) && mana == 0)
        {
            ManaOutSound = RuntimeManager.CreateInstance("event:/SFX/UI/Level/Mana/UI_Mana_Out_(Beep_002)");
            ManaOutSound.start();
            ManaOutSound.release();
        }
    }



    private void ConsumeMana()
    {
        mana--;
        Messenger.Default.Publish(new ManaChangeMessage(mana));
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
