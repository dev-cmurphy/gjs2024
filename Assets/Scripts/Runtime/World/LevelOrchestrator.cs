using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// S'occupe de faire spawn des ennemis selon plusieurs critères
    /// (proximité du joueur, innocence?)
    /// </summary>
    public class LevelOrchestrator : MonoBehaviour
    {

        private static List<EnemyActivator> _activators = new List<EnemyActivator>();


        private void FixedUpdate()
        {
            for(int i = 0; i <  _activators.Count; i++)
            {
                _activators[i].Check();
            }
        }

        public static void RegisterActivator(EnemyActivator enemyActivator)
        {
            _activators.Add(enemyActivator);
        }

        public static void UnregisterActivator(EnemyActivator enemyActivator)
        {
            _activators.Remove(enemyActivator);
        }
    }
}