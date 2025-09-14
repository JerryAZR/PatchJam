using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PressurePlate : OnOffSwitch
{
    public override bool On => Detected >= Threshold;

    private bool _prevState;

    public float Threshold => _threshold;
    [SerializeField] private float _threshold;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private SpriteRenderer _renderer;

    public float Detected { get; private set; }

    private Collider2D _collider;

    protected void Start()
    {
        _collider = GetComponent<Collider2D>();
        _prevState = false;
    }

    protected void Update()
    {
        ContactFilter2D filter = new ContactFilter2D();
        filter.NoFilter();
        List<Collider2D> results = new();
        _collider.Overlap(filter, results);

        float totalWeight = 0;

        foreach (Collider2D obj in results)
        {
            if (obj.TryGetComponent(out IWeighted weighted))
            {
                totalWeight += weighted.Weight;
                // Debug.Log($"Adding weight {weighted.Weight}; total = {totalWeight}");
            }
        }

        Detected = totalWeight;

        if (On != _prevState)
        {
            Debug.Log($"Pressure plate state changed: {On} -> {_prevState}");
            _prevState = On;

            _renderer.sprite = On ? _onSprite : _offSprite;
        }
    }
}
