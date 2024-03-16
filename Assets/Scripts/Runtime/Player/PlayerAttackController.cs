using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace kc.runtime
{
    public class PlayerAttackController : MonoBehaviour
    {
        [SerializeField]
        private float _projectileSpeed;

        [SerializeField]
        private float _firingRate;

        [SerializeField]
        private int _damage;

        [SerializeField]
        private float _bulletLifetime = 3f;

        [SerializeField]
        private PlayerProjectile _projectilePrefab;

        [SerializeField]
        private UnityEvent _onShoot;


        private PlayerInput _input;
        private InputAction _shootAction;

        private float _fireTimer = 0;

        // Use this for initialization
        void Awake()
        {

            _input = GetComponent<PlayerInput>();
            _shootAction = _input.actions["Shoot"];

        }

        private void Update()
        {
            _fireTimer += Time.deltaTime;

        }

        private void FixedUpdate()
        {
            if (_shootAction.IsPressed())
            {
                TryShoot(new InputAction.CallbackContext()); // You may not need any specific info from CallbackContext here
            }
        }

        private void TryShoot(InputAction.CallbackContext context)
        {
            if (!CanShoot())
                return;


            _onShoot.Invoke();
            _fireTimer = 0;
            PlayerProjectile projectile = Instantiate(_projectilePrefab);

            projectile.Initialize(transform.position + transform.right, transform.right * _projectileSpeed , _damage, _bulletLifetime);
        }

        private bool CanShoot()
        {
            return _fireTimer > 1f / _firingRate;
        }
    }
}