using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NotifyLevelClear()
    {
        if (ProgressManager.Instance == null) return;
        ProgressManager.Instance.SetLevelCleared(gameObject.scene.name);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            string sceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(sceneName);
        }
    }
}
