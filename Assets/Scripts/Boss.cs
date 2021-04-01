using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Vector3 _bossPosition;
    private int _speed = 5;
    private int _rotateSpeed = 60;

    private int _life = 50;

    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _explosion;

    private bool _dead = false;

    private Player _player;
    private SpriteRenderer _spriteRenderer;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpawnManager _spawnManager;
    private CircleCollider2D _collider;

    [SerializeField]
    private AudioClip _takeDamageSFX;
    [SerializeField]
    private AudioClip _takeDamageHomingSFX;

    // Start is called before the first frame update
    void Start()
    {
        _bossPosition = new Vector3(0, 1, 0);

        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_dead == false)
        {
            StartCoroutine(FireLaser());
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }

        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("SpawnManager is NULL");
        }

        _collider = GetComponent<CircleCollider2D>();

        if (_collider == null)
        {
            Debug.LogError("Collider is NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, 1) * _rotateSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, _bossPosition, _speed * Time.deltaTime);

        /*if (transform.position.x > 7f)
        {
            _moveLeft = true;
        }
        else if (transform.position.x < -7)
        {
            _moveLeft = false;
        }*/

        //CalculateMovement();

        

    }

    /*void CalculateMovement()
    {

        if (_moveLeft)
        {
            transform.Translate(new Vector3(-1, 0, 0) * _speed * Time.deltaTime);
        }
        else
        {
            transform.Translate(new Vector3(1, 0, 0) * _speed * Time.deltaTime);
        }
    }*/


    IEnumerator FireLaser()
    {
        while (_dead == false)
        {
            Instantiate(_bullet, new Vector3(gameObject.transform.position.x + .5f, gameObject.transform.position.y, 0), gameObject.transform.rotation);
            Instantiate(_bullet, new Vector3(gameObject.transform.position.x - .5f, gameObject.transform.position.y, 0), gameObject.transform.rotation);
            Instantiate(_bullet, new Vector3(gameObject.transform.position.x + .5f, gameObject.transform.position.y + 1, 0), gameObject.transform.rotation);
            Instantiate(_bullet, new Vector3(gameObject.transform.position.x - .5f, gameObject.transform.position.y - 1, 0), gameObject.transform.rotation);

            yield return new WaitForSeconds(.1f);
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player.Damage();
        }
        if (other.tag == "Homing_Bomb")
        {
            Destroy(other.gameObject);

            if (_life > 0)
            {
                AudioSource.PlayClipAtPoint(_takeDamageHomingSFX, new Vector3(0, 1, -10));
                _spawnManager.PowerUpOnHomingBomb(transform.position);
                _life -= 10;
                StartCoroutine(TakeDamage());

            }
            else if (_life <= 0)
            {
                Destroy(this.gameObject, 5f);
                _collider.enabled = false;
                StartCoroutine(Blowup());
                _dead = true;
                _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
                _uiManager.WinSequence();
                _gameManager.GameOver();

            }
        }
            if (other.tag == "Laser")
            {
                Destroy(other.gameObject);

                if (_life > 0)
                {
                    AudioSource.PlayClipAtPoint(_takeDamageSFX, new Vector3(0, 1, -10));
                    _life--;
                    StartCoroutine(TakeDamage());

                }
                else if (_life <= 0)
                {
                    Destroy(this.gameObject, 5f);
                _collider.enabled = false;
                StartCoroutine(Blowup());
                    _dead = true;
                    _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
                    _uiManager.WinSequence();
                    _gameManager.GameOver();


                }
            }

        }

        IEnumerator TakeDamage()
        {
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(.5f);
            _spriteRenderer.color = new Color32(219, 217, 131, 255);
            yield return new WaitForSeconds(.5f);
        }

        IEnumerator Blowup()
        {
            for (int i = 0; i < 10; i++)
            {
                Instantiate(_explosion, this.transform.position + new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-2.0f, 2.0f), 0), Quaternion.identity);
                yield return new WaitForSeconds(.5f);
            }



        }
    
}
