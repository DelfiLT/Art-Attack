using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private KeyCode key;
    [SerializeField] private PowerType power;


    public float CooldownTime { get { return cooldownTime; } }

    bool inCooldown = false;

    public virtual void Use()
    {
        StartCoroutine(StartCooldown());
        Messenger.Default.Publish(new PowerCooldownMessage(cooldownTime, power));
    }

    public virtual bool CanUse()
    {
        return !inCooldown;
    }

    IEnumerator StartCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        inCooldown = false;
    }

    protected void Update()
    {
        if (Input.GetKeyDown(key) && CanUse())
        {
            Use();
        }
    }
}
