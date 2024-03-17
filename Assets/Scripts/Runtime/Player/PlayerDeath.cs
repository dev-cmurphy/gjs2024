
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace kc.runtime
{
    public class PlayerDeath : MonoBehaviour
    {
        [SerializeField]
        private Animator _cameraAnimator;

        // Use this for initialization
        void Start()
        {
            _cameraAnimator.SetTrigger("zoom");
        }

        public void Reload()
        {
            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            // reload current scene ?
            yield return null;


            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}