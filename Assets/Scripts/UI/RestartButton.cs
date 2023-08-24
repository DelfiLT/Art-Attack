using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(SceneController.Instance.RestartLevel);
    }
}
