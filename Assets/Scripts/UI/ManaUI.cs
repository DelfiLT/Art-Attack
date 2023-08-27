using SuperMaxim.Messaging;
using UnityEngine;

public class ManaUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] mana = new GameObject[0];

    void Start()
    {
        Messenger.Default.Subscribe<ManaChangeMessage>(HandleManaChange);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<ManaChangeMessage>(HandleManaChange);
    }

    private void HandleManaChange(ManaChangeMessage message)
    {
        for (int i = 0; i < mana.Length; i++)
        {
            bool enable = i <= message.RemainingMana - 1;
            mana[i].SetActive(enable);
        }
    }
}
