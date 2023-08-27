using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public bool onPlayerAura;

    private void Start()
    {
        onPlayerAura = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Aura"))
        {
            onPlayerAura = true;
        }
    }
}
