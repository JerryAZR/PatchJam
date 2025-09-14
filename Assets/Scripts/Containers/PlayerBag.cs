using System.ComponentModel;
using MoreMountains.CorgiEngine;
using UnityEngine;

public class PlayerBag : ContainerBase
{
    public static PlayerBag Instance { get; private set; }

    public override Vector3 Position =>
        LevelManager.Instance.Players[0].transform.position;

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
