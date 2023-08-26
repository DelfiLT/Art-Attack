using System.Collections;
using UnityEngine;

public abstract class Power : MonoBehaviour
{
    [SerializeField] private float cooldownTime;
    [SerializeField] private KeyCode key;


    bool inCooldown = false;

    public virtual void Use()
    {
        StartCoroutine(StartCooldown());
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
