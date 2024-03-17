using System.Collections;
using UnityEngine;

namespace kc.runtime.Assets.Scripts.Runtime.Enemy
{
    public class EnemyHealthSystem : MonoBehaviour
    {
        [SerializeField]
        private AK.Wwise.Event _deathSound;

        [SerializeField]
        private int _maxHealth;

        private Animator _animator;

        private int _health;

        void Awake()
        {
            _animator = GetComponent<Animator>();
            _health = _maxHealth;
        }

        public void loseHealth(int damage, bool stomp = false)
        {
            _health -= damage;
            Debug.Log("Enemy Health: " + _health);

            _deathSound.Post(gameObject);
            if (_health <= 0)
            {
                TriggerDeath(stomp);
            }
        }

        private void TriggerDeath(bool stomp)
        {
            this.enabled = false;

            InnocenceController.Kill();


            if (stomp)
            {
                _animator.SetTrigger("stomp");
            }
            else
            {
                _animator.SetTrigger("death");
            }
        }

        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}