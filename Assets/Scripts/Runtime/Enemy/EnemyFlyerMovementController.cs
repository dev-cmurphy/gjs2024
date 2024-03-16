using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe du mouvement d'un ennemi qui pourchasse le joueur en volant
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyFlyerMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private Vector2 _direction;

        private Rigidbody2D _rigidbody;

        private Collider2D _currentCollider;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsPlayerInSight())
            {
                // Dive on player
                _direction = new Vector2(_player.transform.position.x - transform.position.x, _player.transform.position.y - transform.position.y).normalized;
            }
            else
            {
                // Move horizontally
                _direction = new Vector2(-1, 0);
            }

            _rigidbody.velocity = _direction * _speed;
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
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider == _currentCollider)
            {
                _currentCollider = null;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, _direction * 2f);
        }

        private bool IsPlayerInSight()
        {
            float distance = new Vector2(_player.transform.position.x - transform.position.x, _player.transform.position.y - transform.position.y).magnitude;

            return distance < 10f;
        }
    }
}