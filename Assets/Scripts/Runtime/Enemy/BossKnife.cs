using System.Collections;
using UnityEngine;

namespace kc.runtime.Assets.Scripts.Runtime.Enemy
{
    public class BossKnife : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private int _damage;
        private float _lifeTime;
        private float _lifeTimer;


        public void Initialize(Vector2 spawnPoint, Vector2 velocity, int damage, float lifetime)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _rigidbody.isKinematic = true;
            _damage = damage;
            _rigidbody.position = spawnPoint;

            _rigidbody.velocity = velocity;
            _lifeTime = lifetime;
            _lifeTimer = 0;
        }

        void Update()
        {
            _lifeTimer += Time.deltaTime;

            if (_lifeTimer > _lifeTime)
            {
                _lifeTimer = 0;
                TriggerDestruction();
            }
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var player = collider.GetComponentInParent<PlayerHealth>();
            if (player)
            {
                // Damage player
                player.Damage(_damage);
                TriggerDestruction();
            }
        }

        public void TriggerDestruction()
        {
            Destroy(this.gameObject);
        }
    }
}