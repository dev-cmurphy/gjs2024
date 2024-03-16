using kc.runtime.Assets.Scripts.Runtime.Enemy;
using System.Collections;
using Unity.VisualScripting.YamlDotNet.Serialization;
using UnityEngine;

namespace kc.runtime
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerProjectile : MonoBehaviour
    {
        private int _damage;
        private Rigidbody2D _body;
        private float _lifeTime;
        private float _lifeTimer;

        public void Initialize(Vector2 spawnPoint, Vector2 velocity, int damage, float lifetime)
        {
            _body = GetComponent<Rigidbody2D>();
            _body.isKinematic = true;
            _damage = damage;
            _body.position = spawnPoint;

            _body.velocity = velocity;
            _lifeTime = lifetime;
            _lifeTimer = 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var enemy = collision.GetComponentInParent<EnemyHealthSystem>();
            if (enemy)
            {
                // damage enemy
                enemy.loseHealth(_damage);
                TriggerDestruction();
            }
        }

        public void TriggerDestruction()
        {
            Destroy(this.gameObject);
        }

        private void Update()
        {
            _lifeTimer += Time.deltaTime;

            if (_lifeTimer > _lifeTime)
            {
                _lifeTimer = 0;
                TriggerDestruction();
            }
        }

        // lifetime : kill after X time or on collision
    }
}