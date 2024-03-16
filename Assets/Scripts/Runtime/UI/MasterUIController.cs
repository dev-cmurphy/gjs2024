using System.Collections;
using TMPro;
using UnityEngine;

namespace kc.runtime
{
    public class MasterUIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _killCount;


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            _killCount.text = InnocenceController.GetGuilt().ToString();
        }
    }
}