using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class AutoMovingPlatform : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _width;
    [SerializeField] private LayerMask _obstacleLayers;

    private Vector3 _forward = Vector3.right;

    void Update()
    {
        // Check if can move forward
        float halfwidth = _width / 2;
        float delta = _speed * Time.deltaTime;

        if (Physics2D.Raycast(
            transform.position, _forward, halfwidth + delta, _obstacleLayers
        ))
        {
            // Turn around
            _forward = -_forward;
        }
        else
        {
            transform.position = transform.position + delta * _forward;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Vector3 from = transform.position + _width / 2 * Vector3.left;
        Vector3 to = transform.position + _width / 2 * Vector3.right;
        Gizmos.DrawLine(from, to);
    }
#endif
}
