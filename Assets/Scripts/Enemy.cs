using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _speed = 4;
    private Player _player;

    private float _fireRate = 3f;
    private float _canFire = -1f;

    [SerializeField]
    private GameObject _eLaserDouble;

    [SerializeField]
    private AudioClip _explosionSFX;

    private Animator _animator;
    private BoxCollider2D _collider;
    private AudioSource _audioSource;

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
        CalculateMovement();

        if (Time.time > _canFire)
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

    }

    void CalculateMovement()
    {
        transform.Translate(new Vector3(0, -1, 0) * _speed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            transform.position = new Vector3(Random.Range(-9.5f, 9.5f), 8, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
            _speed = 0;
            _animator.SetTrigger("OnEnemyDeath");
            _audioSource.Play();
            _collider.enabled = false;
            Destroy(this.gameObject, 2.5f);
        }

        if (other.tag == "Laser")
        {
            _player.AddScore(10);
            Destroy(other.gameObject);

            _speed = 0;

            _animator.SetTrigger("OnEnemyDeath");

            _audioSource.Play();

            _collider.enabled = false;

            Destroy(this.gameObject, 2.5f);
        }
    }
}
