using System.Collections;
using UnityEngine;

namespace kc.runtime.Assets.Scripts.Runtime.Enemy
{
    public class BossOrchestrator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _door;

        [SerializeField]
        private GameObject _boss;

        [SerializeField]
        private SpriteRenderer _bossSprite;

        [SerializeField]
        private Sprite _spriteOursGentil;

        [SerializeField]
        private Sprite _spriteOursMedium;

        [SerializeField]
        private Sprite _spriteOursFache;

        [SerializeField]
        private int _timeUntilSecondPhase;

        [SerializeField]
        private int _timeUntilThirdPhase;

        [SerializeField]
        private int _timeEnd;

        private float _timer;
        private float _jumperTimer;
        private float _flyerTimer;

        // Knives
        [SerializeField]
        private float _knifeSpeed;

        [SerializeField]
        private int _damage;

        [SerializeField]
        private float _knifeLifetime = 3f;

        [SerializeField]
        private BossKnife _knifePrefab;

        private Vector2 _direction;

        void Awake()
        {

        }

        void Update()
        {
            _timer += Time.deltaTime;
            _jumperTimer += Time.deltaTime;
            _flyerTimer += Time.deltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                StartBossFight();
            }
        }

        public void StartBossFight()
        {
            StartCoroutine(BossCoroutine());
        }

        private IEnumerator BossCoroutine()
        {
            // Close door
            _door.SetActive(true);

            // Set Camera
            yield return new WaitForSeconds(1f);

            // Innocent run
            if (InnocenceController.GetGuilt() == 0)
            {
                // Change music

                // Boss appears
                _bossSprite.sprite = _spriteOursGentil;
                _boss.SetActive(true);

                // Spawn confettis

                yield return new WaitForSeconds(10f);
            }
            // Guilty run
            else
            {
                // Change music

                // Boss appears
                if (InnocenceController.GetGuilt() > 100)
                {
                    _bossSprite.sprite = _spriteOursFache;
                }
                else
                {
                    _bossSprite.sprite = _spriteOursMedium;
                }
                _boss.SetActive(true);

                // Pause
                yield return new WaitForSeconds(3.5f);

                while (_timer < _timeEnd)
                {
                    TryShoot();
                    yield return new WaitForSeconds(3.5f);

                    if (InnocenceController.GetGuilt() > 40 && _timer > _timeUntilSecondPhase && (_jumperTimer > 20f))
                    {
                        // Spawn some jumpers

                        _jumperTimer = 0;
                    }

                    if (InnocenceController.GetGuilt() > 100 && _timer > _timeUntilThirdPhase && (_flyerTimer > 20f))
                    {
                        // Spawn some flyers

                        _flyerTimer = 0;
                    }
                }
            }            
        }

        private void TryShoot()
        {
            BossKnife knife = Instantiate(_knifePrefab);

            _direction = new Vector2(PlayerMovementController.PlayerPosition().x - _boss.transform.position.x,
                                     PlayerMovementController.PlayerPosition().y - _boss.transform.position.y).normalized;

            knife.Initialize(_boss.transform.position, _direction * _knifeSpeed, _damage, _knifeLifetime);

            //knife.Initialize(new Vector2(_boss.transform.position.x + 1, _boss.transform.position.y - 1), _direction * _knifeSpeed, _damage, _knifeLifetime);

            //knife.Initialize(new Vector2(_boss.transform.position.x - 1, _boss.transform.position.y + 1), _direction * _knifeSpeed, _damage, _knifeLifetime);
        }
    }
}