using UnityEngine;

public class LevelExit : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NotifyLevelClear()
    {
        ProgressManager.Instance.SetLevelCleared(gameObject.scene.name);
    }
}
