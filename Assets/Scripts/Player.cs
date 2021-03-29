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

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    [SerializeField]
    private int _score = 0;

    [SerializeField]
    private AudioClip _laserSFX;
    private AudioSource _audioSource;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;

    private bool _isTripleShotActive;
    private bool _isSpeedBoostActive;
    private bool _isShieldActive;



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

    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }


    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        if (_isSpeedBoostActive == false)
        {
            transform.Translate(direction * _speed * Time.deltaTime);
        }
        else
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
        }
        else
        {
            _offset = new Vector3(0, 1.05f, 0);
            Instantiate(_laser, transform.position + _offset, Quaternion.identity);
        }

        _audioSource.Play();
 

    }

    public void Damage()
    {

        if (_isShieldActive == true)
        {
            _isShieldActive = false;
            _shield.SetActive(false);
            return;
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);


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
                

            if (_lives < 1)
            {
                
                _spawnManager.PlayerDeath();
                _gameManager.GameOver();
                Destroy(this.gameObject);
            }
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
        _shield.SetActive(true);
    }

    //add 10 to score
    //communicate with UI to update score

    public void AddScore(int plusScore)
    {
        _score = _score += plusScore;
        _uiManager.UpdateScore(_score);

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
