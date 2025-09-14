using System.Collections.Generic;
using UnityEngine;

public class WeightPlatformBase : MonoBehaviour
{
    [SerializeField] private Collider2D _bottomCollider;
    [SerializeField] private LayerMask _collisionMask;
    [SerializeField] private Collider2D _weightZone;

    public float RawWeight { get; private set; }

    public bool CanRise => transform.localPosition.y < 0;
    public bool CanFall { get; private set; }
    private Collider2D[] _obstacle = new Collider2D[1];

    void Update()
    {
        UpdateWeight();

        ContactFilter2D filter = new();
        filter.useTriggers = false;
        filter.SetLayerMask(_collisionMask);
        CanFall = _bottomCollider.Overlap(filter, _obstacle) == 0;
        // if (!CanFall) Debug.Log($"{name} cannot fall due to {_obstacle[0].name} ({_obstacle[0].GetInstanceID()})");
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
}
