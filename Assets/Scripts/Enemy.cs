using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _speed = 4;
    [SerializeField]
    private int _siderSpeedX = 5;
    [SerializeField]
    private int _siderSpeedY = 1;
    private Player _player;

    private float _fireRate = 3f;
    private float _canFire = -1f;

    private float _shieldHealth;

    [SerializeField]
    private int _enemyID;

    [SerializeField]
    private GameObject _eLaserDouble;
    [SerializeField]
    private GameObject _eLaserSingle;
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private AudioClip _explosionSFX;

    private Animator _animator;
    private BoxCollider2D _collider;
    private AudioSource _audioSource;

    private bool _goingLeft;

    private bool _isShieldActive;

    private bool _dead;


    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _animator = GetComponent<Animator>();

        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }

        _collider = GetComponent<BoxCollider2D>();

        if (_collider == null)
        {
            Debug.LogError("Collider is NULL");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL");
        }
        else
        {
            _audioSource.clip = _explosionSFX;
        }


    }
    // Update is called once per frame
    void Update()
    {

        if (_enemyID == 0)
        {
            CalculateMovementE0();
        }
        else if (_enemyID == 1)
        {
            if (transform.position.x > 8)
            {
                _goingLeft = true;
            }
            else if (transform.position.x < -8)
            {
                _goingLeft = false;
            }

            CalculateMovementE1();
        }

        ShieldHealth();
        

        if (Time.time > _canFire && _dead == false)
        {
            if (_enemyID == 0)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                GameObject enemyLaser = Instantiate(_eLaserDouble, transform.position, Quaternion.identity);
                Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
            else if (_enemyID == 1)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                StartCoroutine(RapidShot());
            }

        }

    }

    IEnumerator RapidShot()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 Offset = new Vector3(0, -0.8f, 0);
            GameObject enemyLaser = Instantiate(_eLaserSingle, transform.position + Offset, Quaternion.identity);
            Laser lasers = enemyLaser.GetComponent<Laser>();

            lasers.AssignEnemyLaser();

            yield return new WaitForSeconds(0.2f);
        }
    }
    void CalculateMovementE0()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            Destroy(this.gameObject);
        }
    }

    void CalculateMovementE1()
    {
        if (_goingLeft == true)
        {
            transform.Translate(new Vector3((-1 * _siderSpeedX), (-1 * _siderSpeedY), 0) * Time.deltaTime);
        }
        else if (_goingLeft == false)
        {
            transform.Translate(new Vector3((1 * _siderSpeedX), (-1 * _siderSpeedY), 0) * Time.deltaTime);
        }

        if (transform.position.y < -7f)
            Destroy(this.gameObject);

    }

    public void HasShield()
    {
        _isShieldActive = true;
        _shieldHealth = 1;
        _shield.SetActive(true);
    }

    public void ShieldHealth()
    {
        if (_shieldHealth <= 0)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if (_isShieldActive == true)
            {
                _shieldHealth--;
            }
            else
            {
                _player.Damage();
                _speed = 0;
                _siderSpeedX = 0;
                _siderSpeedY = 0;
                _dead = true;
                _animator.SetTrigger("OnEnemyDeath");
                _audioSource.Play();
                _collider.enabled = false;
                Destroy(this.gameObject, 2.5f);
            }

        }

        if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);

            if (_isShieldActive == true)
            {
                _shieldHealth--;
            }
            else
            {
                _player.AddScore(10);
                _speed = 0;
                _siderSpeedX = 0;
                _siderSpeedY = 0;
                _dead = true;

                _animator.SetTrigger("OnEnemyDeath");

                _audioSource.Play();

                _collider.enabled = false;

                Destroy(this.gameObject, 2.5f);
            }

        }

        if (other.tag == "Homing_Bomb")
        {
            Destroy(other.gameObject);

            _player.AddScore(10);
            _speed = 0;
            _siderSpeedX = 0;
            _siderSpeedY = 0;
            _dead = true;

            _animator.SetTrigger("OnEnemyDeath");

            _audioSource.Play();

            _collider.enabled = false;
            SpawnManager spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
            spawnManager.PowerUpOnHomingBomb(transform.position);
            Destroy(this.gameObject, 2.5f);
        }
    }
}
