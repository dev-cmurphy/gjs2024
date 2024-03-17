using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// Fait le suivi de l'innocence du joueur
    /// </summary>
    public class InnocenceController : MonoBehaviour
    {
        private static InnocenceController _instance;

        [SerializeField]
        private int _guilt;

        [SerializeField]
        private int _guiltPerKill;

        [SerializeField]
        private List<AkSwitch> _switches;

        [SerializeField]
        private GameObject _soundObject;

        private void Awake()
        {
             _instance = this;
        }

        public static void CommitAct(int gravity)
        {
            _instance._guilt += gravity;
        }

        public static void Kill()
        {
            _instance._guilt += _instance._guiltPerKill;
        }

        public static int GetGuilt()
        {
            return _instance._guilt;
        }

        private void Update()
        {
            for (int i = 0; i < _switches.Count; i++)
            {
                if (!_switches[i].gameObject.activeSelf)
                {
                    if (_guilt / 13 >= i)
                    {
                        _switches[i].gameObject.SetActive(true);
                        _switches[i].data.SetValue(_soundObject);
                        Debug.Log($"Setting {i}");
                    }
                }
            }
        }
    }
}
