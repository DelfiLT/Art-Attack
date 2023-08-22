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
        SceneManager.LoadScene(sceneName);
    }

    public void RestartLevel()
    {
        CheckCurrentLevel();

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
        if (string.IsNullOrEmpty(currentLevel))
        {
            currentLevel = SceneManager.GetActiveScene().name;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
