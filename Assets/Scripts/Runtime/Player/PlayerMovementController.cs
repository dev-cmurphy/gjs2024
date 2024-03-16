using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe du mouvement 2D plateforming du joueur
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private float _jumpForce;

        [SerializeField]
        private float _speed;

        [SerializeField]
        private float _maxJumpTime = 0.5f; // Maximum time the jump force can be applied
        private float _jumpTimeCounter; // How long the jump button has been held down
        private bool _isJumping; // Whether the player is currently holding the jump button after initiating a jump


        private Rigidbody2D _rigidbody;
        private PlayerInput _input;

        private InputAction _moveAction, _jumpAction;

        private Vector2 _lastInput;
        private Collider2D _currentCollider;
        // STATE MACHINE ?

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _input = GetComponent<PlayerInput>();
            _moveAction = _input.actions["Move"];
            _jumpAction = _input.actions["Jump"];
        }

        private void OnEnable()
        {
            _jumpAction.started += _ => StartJump();
            _jumpAction.canceled += _ => EndJump();
        }

        private void OnDisable()
        {
            _jumpAction.started -= _ => StartJump();
            _jumpAction.canceled -= _ => EndJump();
        }

        private void StartJump()
        {
            if (CanJump())
            {
                _isJumping = true;
                _jumpTimeCounter = _maxJumpTime;
                _rigidbody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
        }

        private void EndJump()
        {
            _isJumping = false;
        }

        private void Update()
        {
            // Movement
            _lastInput = _moveAction.ReadValue<Vector2>();
            _rigidbody.velocity = new Vector2(_lastInput.x * _speed, _rigidbody.velocity.y);

            // Jump Time Extension
            if (_isJumping && _jumpTimeCounter > 0)
            {
                _rigidbody.AddForce(new Vector2(0f, _jumpForce) * _jumpTimeCounter, ForceMode2D.Force);
                _jumpTimeCounter -= Time.deltaTime;
            }


            Debug.Log($"Can jump: {CanJump()}");
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

        private bool CanJump()
        {
            return _currentCollider != null; // or coyote time
        }

        // TODO: implement coyote time
    }
}