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
        Messenger.Default.Subscribe<ElementChangeMessage>(HandleChangeElement);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<ElementChangeMessage>(HandleChangeElement);
    }

    void HandleChangeElement(ElementChangeMessage message)
    {
        if(message.Element == PowerType.Ice)
        {
            StartCoroutine(ChangeToIce());
        }

        //if (message.Element == PowerType.Water)
        //{
        //    StartCoroutine(ChangeToWater());
        //}
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
