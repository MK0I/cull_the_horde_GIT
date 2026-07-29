using UnityEngine;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadNextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextScene >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("No next scene found.");
            return;
        }

        SceneManager.LoadScene(nextScene);
    }
}