using System;
using System.Collections;
using UnityEngine;
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
        private PlayerProjectile _projectilePrefab;


        private PlayerInput _input;
        private InputAction _shootAction;

        private float _fireTimer = 0;

        // Use this for initialization
        void Awake()
        {

            _input = GetComponent<PlayerInput>();
            _shootAction = _input.actions["Shoot"];

            _shootAction.performed += Shoot;
        }

        private void Update()
        {
            _fireTimer += Time.deltaTime;
        }

        private void Shoot(InputAction.CallbackContext context)
        {
            if (!CanShoot())
                return;

            _fireTimer = 0;
            PlayerProjectile projectile = Instantiate(_projectilePrefab);

            projectile.Initialize(transform.position + transform.right, transform.right * _projectileSpeed , _damage);
        }

        private bool CanShoot()
        {
            return _fireTimer > 1f / _firingRate;
        }
    }
}