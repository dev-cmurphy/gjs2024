using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe du mouvement d'un ennemi de base
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BasicEnemyMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private int _direction;

        private Rigidbody2D _rigidbody;

        private Collider2D _currentCollider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground")) // Make sure your ground has the tag "Ground"
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    // Check if the contact is approximately below us
                    if (point.normal.y > 0.75f && collision.collider.CompareTag("Ground"))
                    {
                        _currentCollider = collision.collider;
                        break;
                    }
                    // Check if the contact is approximately to the left
                    else if (point.normal.x > 0.75f && collision.collider.CompareTag("Ground"))
                    {
                        _direction = 1;
                    }
                    // Check if the contact is approximately to the right
                    else if (point.normal.x < -0.75f && collision.collider.CompareTag("Ground"))
                    {
                        _direction = -1;
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider == _currentCollider)
            {
                _currentCollider = null;
            }
        }
    }
}