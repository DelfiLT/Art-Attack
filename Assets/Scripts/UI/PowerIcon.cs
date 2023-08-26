using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerIcon : MonoBehaviour
{
    [SerializeField]
    private Image icon;

    [SerializeField]
    private PowerType Power;

    private bool inCooldown;
    private float cooldownTime;

    private float time;

    void Start()
    {
        Messenger.Default.Subscribe<PowerCooldownMessage>(HandleCooldownStart, (message) => message.Power == Power);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<PowerCooldownMessage>(HandleCooldownStart);
    }

    private void HandleCooldownStart(PowerCooldownMessage message)
    {
        time = 0;
        cooldownTime = message.CooldownTime;
        StartCoroutine(StartCooldown());
    }


    void Update()
    {
        if (inCooldown)
        {
            time += Time.deltaTime;
            icon.fillAmount = time / cooldownTime;
        }
    }

    private IEnumerator StartCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        inCooldown = false;
    }
}
