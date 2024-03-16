using System.Collections;
using UnityEngine;

namespace kc.runtime.Assets.Scripts.Runtime.Enemy
{
    public class EnemyHealthSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject _enemy;

        [SerializeField]
        private int _maxHealth;
        private int _health;

        [SerializeField]
        private int _damagePerBullet;

        [SerializeField]
        private int _damagePerStomp;

        private Collider2D _currentCollider;

        void Awake()
        {
            _health = _maxHealth;
        }

        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                foreach (ContactPoint2D point in collision.contacts)
                {
                    // Check if the contact is approximately above us
                    if (point.normal.y < -0.75f && collision.collider.CompareTag("Player"))
                    {
                        loseHealth(_damagePerStomp);
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

        public void loseHealth(int damage)
        {
            _health -= damage;
            Debug.Log("Enemy Health: " + _health);

            if (_health <= 0)
            {
                Destroy(_enemy);
            }
        }
    }
}