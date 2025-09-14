using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WeightPlatformPair : MonoBehaviour
{
    [SerializeField] private WeightPlatformBase _platform1;
    [SerializeField] private WeightPlatformBase _platform2;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float _resolution;
    private float _ropeLength;
    public bool Moving { get; private set; }

    void Start()
    {
        _ropeLength = 0 - _platform1.transform.localPosition.y -
            _platform2.transform.localPosition.y;
    }

    void Update()
    {
        float weight1 = _platform1.RawWeight;
        float weight2 = _platform2.RawWeight;
        float weightDiff = Mathf.Abs(weight1 - weight2);
        WeightPlatformBase heavyPlatform, lightPlatform;
        (heavyPlatform, lightPlatform) = weight1 > weight2 ?
            (_platform1, _platform2) : (_platform2, _platform1);

        if (weightDiff > _resolution)
        {
            if (lightPlatform.CanRise && heavyPlatform.CanFall)
            {
                float maxDelta = Mathf.Clamp(
                    weightDiff * _speedMultiplier, 0, _maxSpeed
                ) * Time.deltaTime;
                // Move light platform up first
                Vector3 lightTarget = lightPlatform.transform.localPosition;
                lightTarget.y = 0;
                lightPlatform.transform.localPosition = Vector3.MoveTowards(
                    lightPlatform.transform.localPosition, lightTarget, maxDelta);
                // Next we move the heavy one down
                Vector3 heavyTarget = heavyPlatform.transform.localPosition;
                heavyTarget.y = -_ropeLength - lightPlatform.transform.localPosition.y;
                heavyPlatform.transform.localPosition = heavyTarget;
                Moving = true;
                // Debug.Log($"Moving by {maxDelta}");
            }
            else
            {
                Moving = false;
                // Debug.Log($"Cannot move. Rise={lightPlatform.CanRise}; Fall={heavyPlatform.CanFall}");
            }
        }
        else
        {
            Moving = false;
            // Debug.Log($"Weight difference {weightDiff} < {_resolution}. Not moving");
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position - new Vector3(0, 0.5f, 0), new Vector3(3, 1, 0));
    }
#endif
}
