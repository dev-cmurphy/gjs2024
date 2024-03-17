using System.Collections;
using UnityEngine;

namespace kc.runtime
{
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField]
        private AK.Wwise.Event _startWalkEvent, _stopWalkEvent, _jumpEvent, _touchGroundEvent, _shootEvent;

        [SerializeField]
        private Rigidbody2D _body;

        [SerializeField]
        private AkGameObj _akGameObj;

        private bool _isWalking;

        [SerializeField]
        private PlayerMovementController _movement;

        // Use this for initialization
        void Start()
        {
            _isWalking = false;
        }

        // Update is called once per frame
        void Update()
        {
            if( Mathf.Abs(_body.velocity.x) > 0.1f &&
                Mathf.Abs(_body.velocity.y) < 0.1f && 
                !_isWalking)
            {
                _isWalking = true;
                _startWalkEvent.Post(gameObject);
            }
            else if ((
                Mathf.Abs(_body.velocity.x) < 0.1f ||
                Mathf.Abs(_body.velocity.y) > 0.1f ) && 
                _isWalking)
            {
                _isWalking = false;
                _stopWalkEvent.Post(gameObject);
            }
        }

        private void OnDestroy()
        {
            _stopWalkEvent.Post(gameObject);
        }

        public void Jump()
        {
            _jumpEvent.Post(gameObject);
        }

        public void TouchGround()
        {
            _touchGroundEvent.Post(gameObject);
        }

        public void Shoot()
        {
            _shootEvent.Post(gameObject);
        }
    }
}