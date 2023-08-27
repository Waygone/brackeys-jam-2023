using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    private int _currentLevel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    public bool TryAdvanceToNextLevel()
    {
        if (_currentLevel + 1 >= SceneManager.sceneCount)
        {
            return false;
        }

        SceneManager.LoadScene(++_currentLevel);
        return true;
    }
    public bool TryJumpToLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= SceneManager.sceneCount)
        {
            return false;
        }

        SceneManager.LoadScene(levelIndex);
        return true;
    }
}