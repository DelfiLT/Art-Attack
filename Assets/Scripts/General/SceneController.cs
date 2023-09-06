using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private string currentLevel;

    [SerializeField]
    private List<string> levelsList = new();

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoadScene(string sceneName)
    {
        currentLevel = sceneName;
        GameManager.Instance.ResetPowers();
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        CheckCurrentLevel();
        GameManager.Instance.ResetPowers();
        SceneManager.LoadScene(currentLevel);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void GoToNexLevel()
    {
        CheckCurrentLevel();

        int index = levelsList.IndexOf(currentLevel) + 1;
        if (index < levelsList.Count)
        {
            SceneManager.LoadScene(levelsList[index]); ;
        }
        else
        {
            GoToMainMenu();
        }
    }

    private void CheckCurrentLevel()
    {
        currentLevel = SceneManager.GetActiveScene().name;
    }

    public bool IsLevelCompleted(string sceneName)
    {
        if (string.IsNullOrEmpty(currentLevel))
        {
            CheckCurrentLevel();
        }
        int currIndex = levelsList.IndexOf(currentLevel);
        int otherIndex = levelsList.IndexOf(sceneName);

        return otherIndex < currIndex;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
