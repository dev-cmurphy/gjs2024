using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace kc.runtime
{
    public class MasterUIController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _killCount;

        [SerializeField]
        private PlayerHealth _health;

        [SerializeField]
        private Slider _slider;

        // Use this for initialization
        void Start()
        {
            _slider.maxValue = _health.CurrentHealth();
            _health.OnDamage.AddListener((val) => {_slider.value = val; });
            _health.OnHeal.AddListener((val) => { _slider.value = val; });
        }

        // Update is called once per frame
        void Update()
        {
            _killCount.text = InnocenceController.GetGuilt().ToString();
        }
    }
}