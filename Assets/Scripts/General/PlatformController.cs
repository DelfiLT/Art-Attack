using UnityEngine;

public class PlatformController : MonoBehaviour
{
    [Header("Platform types")]
    [SerializeField] private GameObject ice;
    [SerializeField] private GameObject water;
    [Header("Platform values")]
    [SerializeField] public PlatformType defaultState;
    
    public PlatformType CurrentState { get; private set; }

    void Start()
    {
        SetState(defaultState);
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
}
