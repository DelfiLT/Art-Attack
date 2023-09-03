using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField]
    private string sceneToLoad;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadLevel);
    }

    private void LoadLevel()
    {
        if (!string.IsNullOrEmpty(sceneToLoad))
        {
            SceneController.Instance.LoadScene(sceneToLoad);
        }
    }
}
