using SuperMaxim.Messaging;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    [SerializeField] int maxMana = 3;
    [SerializeField] KeyCode key = KeyCode.LeftShift;

    int mana;
    Power currentPower;

    HashSet<PlatformController> platforms = new();

    private void Start()
    {
        mana = maxMana;
        Messenger.Default.Publish(new ManaChangeMessage(mana));
        Messenger.Default.Subscribe<LearnedPowerMessage>(EnablePower);

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
        }
        currentPower = power;
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
                    }
                    break;
                default:
                    break;
            }
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
