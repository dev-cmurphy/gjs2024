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

        private Rigidbody2D _rigidbody;
        private PlayerInput _input;

        private InputAction _moveAction, _jumpAction;

        private Vector2 _lastInput;
        private bool _isGrounded;
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
            _jumpAction.performed += _ => Jump();
        }

        private void OnDisable()
        {
            _jumpAction.performed -= _ => Jump();
        }

        private void Update()
        {
            // Movement
            _lastInput = _moveAction.ReadValue<Vector2>();
            _rigidbody.velocity = new Vector2(_lastInput.x * _speed, _rigidbody.velocity.y);
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

        // TODO: implement coyote time
    }
}