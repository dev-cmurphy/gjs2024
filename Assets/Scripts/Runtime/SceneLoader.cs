using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace kc.runtime.Assets.Scripts.Runtime
{
    public class SceneLoader : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadGame()
        {

            SceneManager.LoadScene("SampleScene");
        }
    }
}