using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe du mouvement d'un ennemi qui pourchasse le joueur en sautant
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyJumperMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _direction;

        [SerializeField]
        LayerMask _groundLayer;

        private Rigidbody2D _rigidbody;

        private Collider2D _currentCollider;

        private bool shouldJump;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Movement direction
            _direction = Mathf.Sign(_player.transform.position.x - transform.position.x);

            // Check for y-axis movement
            bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << _player.gameObject.layer);

            if (CanJump())
            {
                // Chase player
                _rigidbody.velocity = new Vector2(_direction * _speed, _rigidbody.velocity.y);

                // Jump if there is a gap

                // If ground
                RaycastHit2D groundInFront = Physics2D.Raycast(transform.position, new Vector2(_direction, 0), 2f, _groundLayer);
                // If gap
                RaycastHit2D gapInFront = Physics2D.Raycast(transform.position + new Vector3(_direction, 0, 0), Vector2.down, 2f, _groundLayer);
                // If platform above
                RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, _groundLayer);

                if (!groundInFront.collider && !gapInFront.collider || isPlayerAbove && platformAbove.collider)
                {
                    shouldJump = true;
                }
            }
        }

        private void FixedUpdate()
        {
            if (CanJump() && shouldJump)
            {
                shouldJump = false;

                Vector2 jumpDirection = (_player.position - transform.position).normalized * _jumpForce;

                _rigidbody.AddForce(new Vector2(jumpDirection.x, _jumpForce), ForceMode2D.Impulse);
            }
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

        private bool CanJump()
        {
            return _currentCollider != null;
        }
    }
}