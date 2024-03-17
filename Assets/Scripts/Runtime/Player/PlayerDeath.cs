
using System.Collections;
using UnityEngine;

namespace kc.runtime
{
    public class PlayerDeath : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Reload()
        {
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return null;
        }
    }
}