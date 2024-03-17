using JetBrains.Annotations;
using kc.runtime.Assets.Scripts.Runtime.Enemy;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace kc.runtime
{
    public class PlayerHealth : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth;

        [SerializeField]
        private float _invincibleTime;

        [SerializeField]
        private SpriteRenderer _playerSprite;

        private int _currentHealth;

        private bool _isInvincible = false;
        private float _invincibilityTimer = 0;

        [SerializeField]
        private UnityEvent<float> _onDamage;

        public UnityEvent OnDeath = new UnityEvent();
        [HideInInspector]
        public UnityEvent<float> OnDamage = new UnityEvent<float>();

        private void Awake()
        {
            _currentHealth = _maxHealth;
        }

        public int CurrentHealth()
        {
            return _currentHealth;
        }


        private void OnCollisionEnter2D(Collision2D c)
        {
            var damage = c.collider.GetComponentInParent<EnemyDamage>();
            if (damage) // IN PARENT ?
            {
                bool stomp = false;
                foreach (ContactPoint2D point in c.contacts)
                {
                    // Check if the contact is approximately below us
                    if (point.normal.y > 0.75f)
                    {
                        stomp = true;
                    }
                }

                if (!stomp)
                    Damage(damage.Damage);
                // invincibility frames
            }
        }

        private void Update()
        {
            if (_isInvincible)
            {
                Color c = _playerSprite.color;

                c.a = (_invincibilityTimer % 0.2f) / 0.2f;

                _invincibilityTimer += Time.deltaTime;
                if (_invincibilityTimer > _invincibleTime)
                {
                    _isInvincible = false;
                    c.a = 1;
                }

                _playerSprite.color = c;
            }


        }

        private void Damage(int dmg)
        {
            if (_isInvincible)
            {
                return;
            }

            _invincibilityTimer = 0;
            _isInvincible = true;


            _currentHealth -= dmg;
            _onDamage.Invoke(_currentHealth);
            OnDamage.Invoke(_currentHealth);

            if (_currentHealth <= 0)
            {
                this.enabled = false;
                OnDeath.Invoke();
            }
        }
    }
}