using System.Collections;
using UnityEngine;

public class ProducerBox : ContainerBase
{
    [SerializeField] private SpriteBlock _spriteBlockPrefab;
    [SerializeField] private float _interval;
    [SerializeField] private float _force;
    [SerializeField] private SpriteRenderer _itemRenderer;
    public ItemBaseSO Producing =>
        (Capacity > 0 && ViewSlots[0] != null) ? ViewSlots[0] : null;
    protected override void Start()
    {
        base.Start();
        StartCoroutine(ProduceItem());
    }

    IEnumerator ProduceItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(_interval);
            if (Capacity > 0 && ViewSlots[0] != null)
            {
                if (_itemRenderer != null) _itemRenderer.sprite = ViewSlots[0].Icon;
                if (ViewSlots[0] is PrefabItemSO prefabItem)
                {
                    GameObject produced = Instantiate(prefabItem.Prefab);
                    produced.transform.position = transform.position;
                    if (produced.TryGetComponent(out Rigidbody2D rigidbody))
                    {
                        rigidbody.AddForce(transform.up * _force, ForceMode2D.Impulse);
                    }
                }
                else
                {
                    // Instantiate a generic item
                    SpriteBlock produced = Instantiate(_spriteBlockPrefab);
                    produced.transform.position = transform.position;
                    produced.SetSprite(ViewSlots[0].Icon);
                    produced.GetComponent<Rigidbody2D>().AddForce(transform.up * _force, ForceMode2D.Impulse);
                }
            }
        }
    }
}
