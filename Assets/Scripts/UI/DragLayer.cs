using UnityEngine;

public class DragLayer : MonoBehaviour
{
    public static DragLayer Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }
}