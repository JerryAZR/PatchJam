using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class FallDetection : MonoBehaviour
{
    [SerializeField] private float _minFallSpeed;
    [SerializeField] private UnityEvent _onGrounded;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (
            Mathf.Abs(collision.relativeVelocity.y) > _minFallSpeed &&
            collision.GetContact(0).normal.y > 0
        )
        {
            _onGrounded?.Invoke();
        }
        else
        {
            Debug.Log($"Collision Enter. SpeedY={collision.relativeVelocity.y}; NormalY={collision.GetContact(0).normal.y}");
        }
    }
}
