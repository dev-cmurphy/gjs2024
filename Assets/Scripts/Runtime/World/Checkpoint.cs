using System.Collections;
using UnityEngine;

namespace kc.runtime.Assets.Scripts.Runtime.World
{
    public class Checkpoint : MonoBehaviour
    {

        void Awake()
        {

        }

        void Update()
        {

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Player"))
            {
                TriggerDestruction();
            }
        }

        public void TriggerDestruction()
        {
            Destroy(this.gameObject);
        }
    }
}