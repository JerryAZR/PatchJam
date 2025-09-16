using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class WeightPlatformBase : MonoBehaviour
{
    // [SerializeField] private Collider2D _bottomCollider;
    // [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private Collider2D _weightZone;
    private Rigidbody2D _rigidbody;

    public float RawWeight { get; private set; }

    public bool CanRise => transform.localPosition.y < 0;
    public bool Fall
    {
        set => _rigidbody.bodyType = value ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        UpdateWeight();
    }

    private void UpdateWeight()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        List<Collider2D> results = new();
        _weightZone.Overlap(filter, results);

        float totalWeight = 0;

        foreach (Collider2D obj in results)
        {
            if (obj.TryGetComponent(out IWeighted weighted))
            {
                totalWeight += weighted.Weight;
            }
        }

        RawWeight = totalWeight;
    }

    public void Move(Vector2 position)
    {
        _rigidbody.MovePosition(position);
    }
}
