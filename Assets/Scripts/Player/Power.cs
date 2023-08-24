using System.Collections;
using UnityEngine;

public abstract class Power : MonoBehaviour
    {
    [SerializeField]
    private float cooldownTime;

    bool inCooldown = false;

    IEnumerator StartCooldown()
    {
        inCooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        inCooldown = false;
    }

    public virtual bool CanUse()
    {
        return inCooldown;
    }

    public virtual void Use()
    {
        StartCoroutine(StartCooldown());
    }
}
