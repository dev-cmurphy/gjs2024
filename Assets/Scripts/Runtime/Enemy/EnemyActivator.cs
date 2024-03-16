using System.Collections;
using UnityEngine;

namespace kc.runtime
{
    /// <summary>
    /// Will activate si l'innocence est en dessous d'un niveau X
    /// </summary>
    public class EnemyActivator : MonoBehaviour
    {
        [SerializeField]
        // si la culpabilité est au-dessus de X, l'entité est activée
        private int _guiltTreshold;

        [SerializeField]
        // une fois que le joueur est dans le range, on check l'activation
        private float _activationRange;

        [SerializeField]
        // on ne check plus si le joueur est trop proche
        private float _minActivationRange;

        private void Awake()
        {
            this.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            float dist = Vector2.Distance(transform.position, PlayerMovementController.PlayerPosition());
            if (dist < _activationRange && dist > _minActivationRange)
            {
                CheckActivation();
            }
        }

        private void CheckActivation()
        {
            if (gameObject.activeSelf)
                return;

            if (InnocenceController.GetGuilt() > _guiltTreshold)
            {
                Debug.Log($"Activating {gameObject.name}");
                this.gameObject.SetActive(true);
            }
        }
    }
}