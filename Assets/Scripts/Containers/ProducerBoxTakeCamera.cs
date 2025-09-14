using UnityEngine;

[RequireComponent(typeof(CameraTaker))]
public class ProducerBoxTakeCamera : MonoBehaviour
{
    ProducerBox _producer;
    [SerializeField] private ItemBaseSO _playerDollItem;
    bool _hasCamera;
    void Start()
    {
        _producer = GetComponentInParent<ProducerBox>();
        _hasCamera = false;
    }

    void Update()
    {
        if (!_hasCamera && _producer.Producing == _playerDollItem)
        {
            GetComponent<CameraTaker>().TakeCamera();
            _hasCamera = true;
        }
    }
}
