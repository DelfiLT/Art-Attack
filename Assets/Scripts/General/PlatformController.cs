using SuperMaxim.Messaging;
using System.Collections;
using UnityEditor.VersionControl;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Platform types")]
    [SerializeField] private GameObject ice;
    [SerializeField] private GameObject water;
    [Header("Platform values")]
    [SerializeField] private float changeTime;
    [SerializeField] private PlatformType platformType;

    private PlatformTrigger platformTrigger;

    void Start()
    {
        platformTrigger = GetComponent<PlatformTrigger>();

        Messenger.Default.Subscribe<ElementChangeMessage>(HandleChangeElement);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<ElementChangeMessage>(HandleChangeElement);
    }

    void HandleChangeElement(ElementChangeMessage message)
    {
        if (message.Element == PowerType.Ice && platformType == PlatformType.Water)
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
