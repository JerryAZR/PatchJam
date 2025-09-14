using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void LoadSelectionScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelSelectionRoom");
    }
}
