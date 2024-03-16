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
        private int _maxHealth;
        private int _health;

        [SerializeField]
        private int _damagePerBullet;

        [SerializeField]
        private int _damagePerStomp;

        private Rigidbody2D _rigidbody;

        private Collider2D _currentCollider;

        private int _direction;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _health = _maxHealth;

            _direction = 0;
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
                }
            }
            if (collision.collider.CompareTag("Ground"))
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    // Check if the contact is approximately beside us
                    if (point.normal.x > 0.75f && collision.collider.CompareTag("Ground"))
                    {
                        Debug.Log($"Collision to left");
                        _direction = 1;
                    }
                    else if (point.normal.x < -0.75f && collision.collider.CompareTag("Ground"))
                    {
                        Debug.Log($"Collision to right");
                        _direction = -1;
                    }
                }
            }
            if (collision.collider.CompareTag("Player"))
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    // Check if the contact is approximately above us
                    if (point.normal.y > 0.75f && collision.collider.CompareTag("Player"))
                    {
                        loseHealth(_damagePerStomp);
                    }
                }
            }
            if (collision.collider.CompareTag("Bullet")) // Change for wtv the projectile tag is
            {
                loseHealth(_damagePerBullet);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider == _currentCollider)
            {
                _currentCollider = null;
            }
        }

        private bool isFalling()
        {
            return _currentCollider == null;
        }

        private void loseHealth(int damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                // Make the enemy disappear.
            }
        }
    }
}