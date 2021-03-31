using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;
    [SerializeField]
    private GameObject _tripleShot;
    [SerializeField]
    private GameObject _shield;

    [SerializeField]
    private GameObject _thrusterR, _thrusterL;

    private Vector3 _offset;


    [SerializeField]
    private int _speed = 6;
    private int _speedBoost = 2;
    private int _thrusterBoost = 2;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _currentAmmo;

    [SerializeField]
    private int _shieldHealth;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private AudioClip _laserSFX;
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _outOfAmmoSFX;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;
    private SpriteRenderer _shieldSpriteRenderer;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;
    private bool _isThrusterActive;



    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

       _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }

        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UIManager is NULL");
        }

        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("AudioSource is NULL");
        }
        else
        {
            _audioSource.clip = _laserSFX;
        }

        _shieldSpriteRenderer = _shield.GetComponent<SpriteRenderer>();

        if (_shieldSpriteRenderer == null)
        {
            Debug.LogError("Shield Sprite Renderer is NULL");
        }

        
        _currentAmmo = 15;
        UpdateAmmo();

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        ShieldHealth();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

        _currentAmmo = Mathf.Clamp(_currentAmmo, 0, 15);
        _lives = Mathf.Clamp(_lives, 0, 3);


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
           _speed += _thrusterBoost;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speed -= _thrusterBoost;
        }

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else if (_isSpeedBoostActive == true)
        {

            transform.Translate(direction * _speed * _speedBoost * Time.deltaTime);
        }


        if (transform.position.y >= 6)
        {
            transform.position = new Vector3(transform.position.x, 6, 0);
        }
        else if (transform.position.y <= -4)
        {
            transform.position = new Vector3(transform.position.x, -4, 0);
        }

        if (transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.25f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11.3f)
        {
            transform.position = new Vector3(11.22f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShot, transform.position, Quaternion.identity);
            _audioSource.Play();
        }
        else if (_isTripleShotActive == false && _currentAmmo >= 1)
        {
            _currentAmmo--;
            UpdateAmmo();
            _offset = new Vector3(0, 1.05f, 0);
            Instantiate(_laser, transform.position + _offset, Quaternion.identity);
            _audioSource.Play();
        }
        else
        {
            AudioSource.PlayClipAtPoint(_outOfAmmoSFX, transform.position);
            UpdateAmmo();
        }

    }

    public void Reload()
    {
        _currentAmmo = 15;
        _uiManager.UpdateAmmo(_currentAmmo);
    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            _shieldHealth--;

            return;

        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            UpdateThrusters();

            if (_lives < 1)
            {
                _spawnManager.PlayerDeath();
                _gameManager.GameOver();
                Destroy(this.gameObject);
            }
        }

    }

    public void UpdateThrusters()
    {
        if (_lives == 2)
        {
            _thrusterL.SetActive(false);
            _thrusterR.SetActive(true);
        }
        else if (_lives >= 3)
        {
            _thrusterL.SetActive(false);
            _thrusterR.SetActive(false);
        }
        else if (_lives == 1)
        {
            _thrusterL.SetActive(true);
            _thrusterR.SetActive(true);
        }

    }

    public void SpeedUp()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedUpPowerDown());
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDown());
    }

    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldHealth = 3;
        _shield.SetActive(true);
    }

    public void ShieldHealth()
    {
        switch (_shieldHealth)
        {
            case 0:
                _isShieldActive = false;
                _shield.SetActive(false);
                break;
            case 1:
                _shieldSpriteRenderer.color = Color.red;
                break;
            case 2:
                _shieldSpriteRenderer.color = Color.blue;
                break;
            case 3:
                _shieldSpriteRenderer.color = Color.white;
                break;
            default:
                break;
        }
    }

    public void Heal()
    {
        _lives++;
        _uiManager.UpdateLives(_lives);
        UpdateThrusters();
    }

    public void AddScore(int plusScore)
    {
        _score = _score += plusScore;
        _uiManager.UpdateScore(_score);

    }

    public void UpdateAmmo()
    {
        _uiManager.UpdateAmmo(_currentAmmo);
    }

    IEnumerator TripleShotPowerDown()
    {
        yield return new WaitForSeconds(5);
        _isTripleShotActive = false;
    }

    IEnumerator SpeedUpPowerDown()
    {
        yield return new WaitForSeconds(5);
        _isSpeedBoostActive = false;
    }
}
