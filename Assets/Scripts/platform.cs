using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;

public class platform : MonoBehaviour
{
    [SerializeField]
    private GameObject ice;
    [SerializeField]
    private GameObject water;

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
        ice.SetActive(true);
        water.SetActive(false);
        yield return new WaitForSeconds(10f);
        Debug.Log("Volvio a su estado");
        ice.SetActive(false);
        water.SetActive(true);
    }

    IEnumerator ChangeToWater()
    {
        Debug.Log("Elemento cambiado a awa");
        ice.SetActive(false);
        water.SetActive(true);
        yield return new WaitForSeconds(10f);
        Debug.Log("Volvio a su estado");
        ice.SetActive(true);
        water.SetActive(false);
    }
}
