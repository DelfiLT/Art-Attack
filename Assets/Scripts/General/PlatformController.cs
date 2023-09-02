using SuperMaxim.Messaging;
using System;
using System.Collections;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Platform types")]
    [SerializeField] private GameObject ice;
    [SerializeField] private GameObject water;
    [Header("Platform values")]
    //[SerializeField] private float changeTime;
    [SerializeField] public PlatformType defaultState;
    
    public PlatformType CurrentState { get; private set; }

    //private PlatformTrigger platformTrigger;

    void Start()
    {
        SetState(defaultState);

        //Messenger.Default.Subscribe<ElementChangeMessage>(HandleChangeElement);
    }

    private void OnDestroy()
    {
        //Messenger.Default.Unsubscribe<ElementChangeMessage>(HandleChangeElement);
    }

    void HandleChangeElement(ElementChangeMessage message)
    {
        //if (message.Element == PowerType.Ice && platformType == PlatformType.Water)
        //{
        //    StartCoroutine(ChangeToIce());
        //}

        //if (message.Element == PowerType.Water)
        //{
        //    StartCoroutine(ChangeToWater());
        //}
    }

    public bool AlreadyChanged(PowerType power)
    {
        bool alreadyIce = CurrentState == PlatformType.Ice && power == PowerType.Ice;
        bool alreadyWater = CurrentState == PlatformType.Water && power == PowerType.Fire;
        return alreadyIce || alreadyWater;
    }

    public void ChangeElement(PowerType element)
    {
        switch (element)
        {
            case PowerType.Ice:
                CurrentState = PlatformType.Ice;
                break;
            case PowerType.Fire:
                CurrentState = PlatformType.Water;
                break;
            default:
                break;
        }

        SetState(CurrentState);
        Debug.Log($"Plataforma {gameObject.name} cambiada a {CurrentState}");
    }
    
    private void SetState(PlatformType state)
    {
        switch (state)
        {
            case PlatformType.Ice:
                ice.SetActive(true);
                water.SetActive(false);
                break;
            case PlatformType.Water:
                ice.SetActive(false);
                water.SetActive(true);
                break;
            default:
                break;
        }
        CurrentState = state;
    }

    //IEnumerator ChangeToIce()
    //{
    //    ice.SetActive(true);
    //    water.SetActive(false);
    //    yield return new WaitForSeconds(changeTime);
    //    ice.SetActive(false);
    //    water.SetActive(true);
    //}

    //IEnumerator ChangeToWater()
    //{
    //    ice.SetActive(false);
    //    water.SetActive(true);
    //    yield return new WaitForSeconds(changeTime);
    //    ice.SetActive(true);
    //    water.SetActive(false);
    //}
}
