using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace kc.runtime.Assets.Scripts.Runtime.Enemy
{
    public class BossOrchestrator : MonoBehaviour
    {
        [SerializeField]
        private AK.Wwise.Switch _switchFinal, _switchIntro, _switchFight1, _switchFight2, _switchFight3, _switchEnd1, _switchEnd23;

        [SerializeField]
        private GameObject _door;

        [SerializeField]
        private GameObject _boss;

        [SerializeField]
        private GameObject _soundObject;

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

        [SerializeField]
        private GameObject _enemyFlyer, _enemyJumper;

        [SerializeField]
        private Transform _spawner;

        [SerializeField]
        private GameObject _guiltEndGameObject;

        [SerializeField]
        private GameObject _innoEndGameObject;

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

        private void SpawnEnemies(Vector2 pos, int count, GameObject prefab)
        {
            for (int i  = 0; i < count; i++)
            {
                GameObject inst = Instantiate(prefab);
                inst.transform.position = pos + (Random.insideUnitCircle * 3) ;
            }
        }

        private IEnumerator BossCoroutine()
        {
            // Close door
            _door.SetActive(true);
            _switchFinal.SetValue(_soundObject);
            _switchIntro.SetValue(_soundObject);

            // Set Camera
            yield return new WaitForSeconds(1f);

            // Innocent run
            if (InnocenceController.GetGuilt() == 0)
            {
                // Change music

                _switchFight1.SetValue(_soundObject);

                // Boss appears
                _bossSprite.sprite = _spriteOursGentil;
                _boss.SetActive(true);

                yield return new WaitForSeconds(4f);
                // Spawn confettis
                _innoEndGameObject.SetActive(true);

                yield return new WaitForSeconds(10f);

                _switchEnd1.SetValue(_soundObject);
            }
            // Guilty run
            else
            {
                // Change music

                // Boss appears
                if (InnocenceController.GetGuilt() > 100)
                {
                    _switchFight3.SetValue(_soundObject);
                    _bossSprite.sprite = _spriteOursFache;
                }
                else
                {
                    _switchFight2.SetValue(_soundObject);
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

                        SpawnEnemies(_spawner.position, 10, _enemyJumper);
                        _jumperTimer = 0;
                    }

                    if (InnocenceController.GetGuilt() > 100 && _timer > _timeUntilThirdPhase && (_flyerTimer > 20f))
                    {
                        // Spawn some flyers

                        SpawnEnemies(_spawner.position, 10, _enemyFlyer);
                        _flyerTimer = 0;
                    }
                }

                // win

                _switchEnd23.SetValue(_soundObject);
                _guiltEndGameObject.SetActive(true);

            }


            yield return new WaitForSeconds(8);
            SceneManager.LoadScene("End");
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