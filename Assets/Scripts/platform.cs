using SuperMaxim.Messaging;
using UnityEngine;

public class platform : MonoBehaviour
{
    void Start()
    {
        Messenger.Default.Subscribe<ColorChange>(HandleChangeColor);
    }

    private void OnDestroy()
    {
        Messenger.Default.Unsubscribe<ColorChange>(HandleChangeColor);
    }

    void HandleChangeColor(ColorChange message)
    {
        Debug.Log("Color cambiado a "+ message.Color);
    }
}
