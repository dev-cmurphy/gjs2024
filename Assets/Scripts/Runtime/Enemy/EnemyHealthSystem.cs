using System.Collections;
using UnityEngine;

namespace kc.runtime.Assets.Scripts.Runtime.Enemy
{
    public class EnemyHealthSystem : MonoBehaviour
    {
        [SerializeField]
        private int _maxHealth;

        private int _health;

        void Awake()
        {
            _health = _maxHealth;
        }

        public void loseHealth(int damage)
        {
            _health -= damage;
            Debug.Log("Enemy Health: " + _health);

            if (_health <= 0)
            {
                TriggerDeath();
            }
        }

        private void TriggerDeath()
        {
            this.enabled = false;

            InnocenceController.Kill();

            Destroy(this.gameObject);
        }
    }
}