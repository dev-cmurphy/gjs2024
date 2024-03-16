using UnityEngine;

namespace kc.runtime
{

    public class Camera_Follow_Player : MonoBehaviour
    {
        [SerializeField]
        public Transform player;

        // drag ?
        // snap speed ?

        void LateUpdate()
        {
            Vector3 normalized = player.transform.position;
            normalized.y = transform.position.y;
            normalized.z = transform.position.z;
            transform.position = normalized;
        }
    }

}
