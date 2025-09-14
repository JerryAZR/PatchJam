using Unity.VisualScripting;
using UnityEngine;

public class OnOFfElevator : MonoBehaviour
{
    [SerializeField] private Vector3 _OnPosition;
    [SerializeField] private Vector3 _OffPosition;
    [SerializeField] private float _speed;
    [SerializeField] private OnOffSwitch _switch;
    [SerializeField] private Transform _platform;

    void Start()
    {
        _platform.localPosition = _OffPosition;
        if (_switch == null) Debug.LogError("Missing OnOffSwitch");
    }

    void Update()
    {
        float maxDelta = _speed * Time.deltaTime;
        Vector3 target = (_switch != null && _switch.On) ? _OnPosition : _OffPosition;
        _platform.localPosition = Vector3.MoveTowards(_platform.localPosition, target, maxDelta);
    }
}
