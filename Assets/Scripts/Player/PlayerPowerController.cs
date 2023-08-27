using UnityEngine;

public class PlayerPowerController : MonoBehaviour
{
    [SerializeField] int maxMana = 3;
    [SerializeField] KeyCode key = KeyCode.LeftShift;

    int mana;
    Power currentPower;

    private void Start()
    {
        mana = maxMana;
    }

    public void SetCurrentPower(Power power)
    {
        if (currentPower != null)
        {
            currentPower.Unset();
        }
        currentPower = power;
    }

    private void Update()
    {
        if (Input.GetKeyDown(key) && mana > 0 && currentPower != null)
        {
            currentPower.Use();
            mana--;
        }
    }
}
