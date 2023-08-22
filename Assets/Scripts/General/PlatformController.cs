using SuperMaxim.Messaging;
using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Platform types")]
    [SerializeField] private GameObject ice;
    [SerializeField] private GameObject water;
    [Header("Platform values")]
    [SerializeField] private float changeTime;

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
        ice.SetActive(true);
        water.SetActive(false);
        yield return new WaitForSeconds(changeTime);
        ice.SetActive(false);
        water.SetActive(true);
    }

    IEnumerator ChangeToWater()
    {
        ice.SetActive(false);
        water.SetActive(true);
        yield return new WaitForSeconds(changeTime);
        ice.SetActive(true);
        water.SetActive(false);
    }
}
