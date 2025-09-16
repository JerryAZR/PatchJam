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

    void Start()
    {
        _ropeLength = 0 - _platform1.transform.localPosition.y -
            _platform2.transform.localPosition.y;
    }

    void FixedUpdate()
    {
        float weight1 = _platform1.RawWeight;
        float weight2 = _platform2.RawWeight;
        float weightDiff = Mathf.Abs(weight1 - weight2);
        WeightPlatformBase heavyPlatform, lightPlatform;
        (heavyPlatform, lightPlatform) = weight1 > weight2 ?
            (_platform1, _platform2) : (_platform2, _platform1);

        lightPlatform.Fall = false;
        heavyPlatform.Fall = false;

        if (weightDiff > _resolution)
        {
            if (lightPlatform.CanRise)
            {
                float maxDelta = Mathf.Clamp(
                    weightDiff * _speedMultiplier, 0, _maxSpeed
                ) * Time.deltaTime;
                // Move heavy platform down first
                heavyPlatform.Fall = true;
                Vector3 fallTarget = heavyPlatform.transform.localPosition;
                fallTarget.y = -_ropeLength;
                fallTarget = Vector3.MoveTowards(
                    heavyPlatform.transform.localPosition, fallTarget, maxDelta
                );
                heavyPlatform.Move(transform.TransformPoint(fallTarget));

                Vector3 riseTarget = lightPlatform.transform.localPosition;
                riseTarget.y = -_ropeLength - heavyPlatform.transform.localPosition.y;
                lightPlatform.Move(transform.TransformPoint(riseTarget));
            }
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawCube(transform.position - new Vector3(0, 0.5f, 0), new Vector3(3, 1, 0));
    }
#endif
}
