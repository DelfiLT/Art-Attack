using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;

public class platform : MonoBehaviour
{
    void Start()
    {
        Messenger.Default.Subscribe<ElementChange>(HandleChangeElement);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<ElementChange>(HandleChangeElement);
    }

    void HandleChangeElement(ElementChange message)
    {
        if(message.Element == Element.Ice)
        {
            StartCoroutine(ChangeToIce());
        }

        if (message.Element == Element.Water)
        {
            StartCoroutine(ChangeToWater());
        }
    }

    IEnumerator ChangeToIce()
    {
        Debug.Log("Elemento cambiado a hielo");
        yield return new WaitForSeconds(10f);
        Debug.Log("Volvio a su estado");
    }

    IEnumerator ChangeToWater()
    {
        Debug.Log("Elemento cambiado a awa");
        yield return new WaitForSeconds(10f);
        Debug.Log("Volvio a su estado");
    }
}
