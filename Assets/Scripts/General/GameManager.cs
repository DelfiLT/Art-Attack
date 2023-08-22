using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string nextLevel;

    void Start() {}

    void Update() {}

    void LoadNextLevel()
    {
        if(nextLevel != null) SceneManager.LoadScene(nextLevel);
    }
}
