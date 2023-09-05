using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CloseAppButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Close);
    }

    void Close()
    {
        Application.Quit();
    }
}
