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


        public void Initialize(Vector2 spawnPoint, Vector2 velocity, int damage)
        {
            _body = GetComponent<Rigidbody2D>();
            _body.isKinematic = true;
            _damage = damage;
            _body.position = spawnPoint;

            _body.velocity = velocity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<BasicEnemyMovementController>(out var comp))
            {
                // damage enemy
                Debug.Log("Will damage enemy from this place in code.");
            }
        }
    }
}