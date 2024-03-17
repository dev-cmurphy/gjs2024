using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace kc.runtime
{
    public class Camera_Follow_Player : MonoBehaviour
    {
        [SerializeField]
        public Transform player;

        [SerializeField]
        private float _heightThreshold;

        [SerializeField]
        private float _followSpeed = 5f; // Adjust this to control the lerp speed

        private float _baseY; // The initial y position of the camera

        [SerializeField]
        private float _timeAboveThreshold = 0.5f; // Time player needs to be above the threshold to trigger camera movement

        private float _timeSpentAboveThreshold = 0f; // Timer for tracking time spent above the threshold


        private void Awake()
        {
            // Initialize baseY to the starting y position of the camera
            _baseY = transform.position.y;
        }

        void LateUpdate()
        {
            Vector3 targetPosition = transform.position;
            float deltaY = player.transform.position.y - _baseY; // Difference in Y from player to base Y position

            float targetY = _baseY;

            if (Mathf.Abs(deltaY) > _heightThreshold)
            {
                // Increment the timer since the player is above the threshold
                _timeSpentAboveThreshold += Time.deltaTime;

                // Check if the player has been above the threshold for long enough
                if (_timeSpentAboveThreshold >= _timeAboveThreshold)
                {
                    // Check if the player has moved beyond the height threshold from the base y-position
                    if (Mathf.Abs(deltaY) > _heightThreshold)
                    {
                        // Calculate new target Y position with respect to the threshold
                        targetY = _baseY + Mathf.Sign(deltaY) * _heightThreshold;
                    }
                }
            }
            else
            {
                _timeSpentAboveThreshold = 0;
            }

            // Lerp the camera's y position towards the new target Y position
            targetPosition.y = Mathf.Lerp(transform.position.y, targetY, _followSpeed * Time.deltaTime);
            // Maintain the camera's current Z position and follow the player's X position
            targetPosition.x = player.transform.position.x;
            targetPosition.z = transform.position.z;

            // Update the camera's position
            transform.position = targetPosition;
        }
    }
}
