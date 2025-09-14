using MoreMountains.CorgiEngine;
using UnityEngine;

/// <summary>
/// A prefab holding a sprite
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class SpriteBlock : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _itemRenderer;
    public void SetSprite(Sprite sprite)
    {
        _itemRenderer.sprite = sprite;
    }

    void Update()
    {
        if (!LevelManager.Instance.LevelBounds.Contains(transform.position))
        {
            Destroy(gameObject);
        }
    }
}
