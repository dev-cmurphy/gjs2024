using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe du mouvement d'un ennemi de base
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private float _speed;

        private Rigidbody2D _rigidbody;

        private Vector2 _lastInput;
        private bool _isGrounded;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // Move in the direction of the player
            int direction = -1;
            if (_player.transform.position.x > _rigidbody.position.x)
            {
                direction = 1;
            }
            _rigidbody.velocity = new Vector2(direction * _speed, _rigidbody.velocity.y);
        }

        private void Jump()
        {
            if (_isGrounded)
            {
                _rigidbody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
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