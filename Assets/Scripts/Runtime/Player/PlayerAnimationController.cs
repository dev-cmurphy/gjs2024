using System.Collections;
using UnityEngine;

namespace kc.runtime
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Rigidbody2D _body;

        [SerializeField]
        private PlayerMovementController _movement;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _animator.SetFloat("horizontal", _body.velocity.x);

            _animator.SetBool("isAirborne", _movement.IsAirborne());
        }
    }
}