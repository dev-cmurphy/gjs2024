using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe du mouvement d'un ennemi qui pourchasse le joueur en volant
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyFlyerMovementController : MonoBehaviour
    {
        [SerializeField]
        private Transform _player;

        [SerializeField]
        private float _acceleration;

        [SerializeField]
        private Vector2 _direction;

        [SerializeField]
        private float _sightDistance;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (IsPlayerInSight())
            {
                // Dive on player
                _direction = new Vector2(_player.transform.position.x - transform.position.x, _player.transform.position.y - transform.position.y).normalized;
            }
            else
            {
                // Move horizontally
                _direction = new Vector2(_player.transform.position.x - transform.position.x, 0).normalized;
            }

            _rigidbody.AddForce(_direction * _acceleration);
        }

        private bool IsPlayerInSight()
        {
            float distance = new Vector2(_player.transform.position.x - transform.position.x, _player.transform.position.y - transform.position.y).magnitude;

            return distance < _sightDistance;
        }
    }
}