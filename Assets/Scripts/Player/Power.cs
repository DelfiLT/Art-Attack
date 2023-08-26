using System.Collections;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] private float cooldownTime;

    public float CooldownTime { get { return cooldownTime; } }

    bool inCooldown = false;

    public virtual void Use()
    {
        StartCoroutine(StartCooldown());
    }

    public virtual bool CanUse()
    {
        return inCooldown;
    }

    IEnumerator StartCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        inCooldown = false;
    }
}
