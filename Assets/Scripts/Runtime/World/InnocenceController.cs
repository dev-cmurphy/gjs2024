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

        private int _guilt;

        [SerializeField]
        private int _guiltPerKill;

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
    }
}
