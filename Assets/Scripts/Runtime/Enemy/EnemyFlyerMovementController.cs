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
        private float _acceleration;

        [SerializeField]
        private Vector2 _direction;

        [SerializeField]
        private float _sightDistance;

        [SerializeField]
        private Animator _animator;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Vector2 pos = PlayerMovementController.PlayerPosition();
            if (IsPlayerInSight())
            {
                // Dive on player
                _direction = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y).normalized;
            }
            else
            {
                // Move horizontally
                _direction = new Vector2(pos.x - transform.position.x, 0).normalized;
            }

            _rigidbody.AddForce(_direction * _acceleration);

            _animator.SetFloat("horizontal", _rigidbody.velocity.x);
        }

        private bool IsPlayerInSight()
        {
            Vector2 pos = PlayerMovementController.PlayerPosition();
            float distance = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y).magnitude;

            return distance < _sightDistance;
        }
    }
}