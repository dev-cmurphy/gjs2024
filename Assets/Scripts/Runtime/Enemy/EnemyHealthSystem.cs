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

        void Awake()
        {
            _health = _maxHealth;
        }

        void Update()
        {

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