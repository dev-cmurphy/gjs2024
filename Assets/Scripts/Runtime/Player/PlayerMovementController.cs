using kc.runtime.Assets.Scripts.Runtime.Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
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

        [SerializeField]
        private int _stompDamage;

        [SerializeField]
        private float _coyoteTime = 0.82f;

        [SerializeField]
        private float _jumpGroundMinimumDist = 0.1f;

        [SerializeField]
        private UnityEvent _onJump = new UnityEvent();

        [SerializeField]
        private float _maxVel = 3f;


        [SerializeField]
        private UnityEvent _onTouchGround = new UnityEvent();

        private Rigidbody2D _rigidbody;
        private PlayerInput _input;

        private InputAction _moveAction, _jumpAction;

        private Vector2 _lastInput;
        private Collider2D _currentGroundCollider;
        // STATE MACHINE ?

        private float _coyoteTimer;
        private float _lastJumpTimer;

        private Vector2 _startPos;

        private static PlayerMovementController _instance;

        private void Awake()
        {
            _instance = this;
            _startPos = transform.position;
            _rigidbody = GetComponent<Rigidbody2D>();

            _input = GetComponent<PlayerInput>();
            _moveAction = _input.actions["Move"];
            _jumpAction = _input.actions["Jump"];
            _input.actions["Reset"].performed += ResetPlayer;

            _coyoteTimer = 0;
            _lastJumpTimer = 0;

            _isLeft = false;
        }

        private void ResetPlayer(InputAction.CallbackContext obj)
        {
            _rigidbody.position = _startPos;
            _rigidbody.velocity = Vector2.zero;
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
                _onJump.Invoke();
                _lastJumpTimer = 0;
                _isJumping = true;
                _jumpTimeCounter = _maxJumpTime;
                ResetVerticalVelocity();
                _rigidbody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
            }
        }

        private void EndJump()
        {
            _isJumping = false;
        }

        public static Vector2 PlayerPosition()
        {
            return _instance.transform.position;
        }

        private void Update()
        {
            if (_currentGroundCollider == null)
            {
                _coyoteTimer += Time.deltaTime;
            }

            _lastJumpTimer += Time.deltaTime;

            // Movement
            _lastInput = _moveAction.ReadValue<Vector2>();
            _rigidbody.velocity = new Vector2(_lastInput.x * _speed, _rigidbody.velocity.y);

            if (_lastInput.x < 0f)
            {
                _isLeft = true;
            }

            if (_lastInput.x > 0f)
            {
                _isLeft = false;
            }

            // Jump Time Extension
            if (_isJumping && _jumpTimeCounter > 0)
            {
                _rigidbody.AddForce(new Vector2(0f, _jumpForce) * _jumpTimeCounter, ForceMode2D.Force);
                _jumpTimeCounter -= Time.deltaTime;
            }

            _rigidbody.velocity = Vector2.ClampMagnitude(_rigidbody.velocity, _maxVel);
        }

        private void ResetVerticalVelocity()
        {
            Vector2 resetVel = _rigidbody.velocity;
            resetVel.y = 0;
            _rigidbody.velocity = resetVel;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Ground")) // Make sure your ground has the tag "Ground"
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    // Check if the contact is approximately below us
                    if (point.normal.y > 0.75f)
                    {
                        _currentGroundCollider = collision.collider;
                        _onTouchGround.Invoke();
                        break;
                    }
                }
            }
            else if (collision.collider.CompareTag("Enemy"))
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    // Check if the contact is approximately below us
                    if (point.normal.y > 0.75f)
                    {
                        var enemy = collision.collider.GetComponentInParent<EnemyHealthSystem>();
                        if (enemy)
                        {
                            // damage enemy
                            enemy.loseHealth(_stompDamage);
                            ResetVerticalVelocity();
                            _rigidbody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
                        }
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.collider == _currentGroundCollider)
            {
                _currentGroundCollider = null;
                _coyoteTimer = 0;
            }
        }

        public bool IsAirborne()
        {
            return _isJumping;
        }

        private bool CanJump()
        {
            bool isCloseEnoughToGround = false;

            // here, check with a raycast if the player is close enough (the float variable _jumpGroundMinimumDist exists)
            // and assign the result to the boolean

            int layerMask = ~(1 << LayerMask.NameToLayer("Player"));

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _jumpGroundMinimumDist, layerMask);
            if (hit.collider != null)
            {
                isCloseEnoughToGround = hit.collider.CompareTag("Ground") || hit.collider.CompareTag("Enemy");

                if (_rigidbody.velocity.y < 0f)
                {
                    isCloseEnoughToGround = false;
                }
            }

            return isCloseEnoughToGround || _currentGroundCollider != null || (_coyoteTimer < _coyoteTime && _lastJumpTimer > _coyoteTime);
        }

        private bool _isLeft;

        public bool IsLeft()
        {
            return _isLeft;
        }
    }
}