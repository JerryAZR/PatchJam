using System.ComponentModel;
using UnityEngine;

public class PlayerBag : ContainerBase
{
    public static PlayerBag Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}
