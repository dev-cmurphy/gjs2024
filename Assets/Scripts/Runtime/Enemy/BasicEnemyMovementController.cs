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

        private Rigidbody2D _rigidbody;

        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            
            int direction = -1;
            // Fall straight if not grounded
            if (!_isGrounded)
            {
                direction = 0;
            }
            // Move in the direction of the player
            else if (_player.transform.position.x > _rigidbody.position.x)
            {
                direction = 1;
            }

            _rigidbody.velocity = new Vector2(direction * _speed, _rigidbody.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.tag == "Ground") // Make sure your ground has the tag "Ground"
            {
                _isGrounded = true;
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider.tag == "Ground")
            {
                _isGrounded = false;
            }
        }
    }
}