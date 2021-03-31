using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemy; //0 Default, 1 Sider
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private bool _stopSpawning;

    private Vector3 _enemySpawnPos;
    private Vector3 _powerUpSpawnPos;

    private int[] _siderSpawnPos = new int[] { -8, 8 };

    private int[] _highChance = new int[] { 3 }; //3 ammo
    private int[] _mediumChance = new int[] { 0, 1, 2 }; // 0 Triple, 1 Speed, 2 Shield
    private int[] _lowChance = new int[] { 4, 6 }; // 4 Health, 6 AmmoMaxDown
    private int[] _veryLowChance = new int[] { 5 }; // 5 HomingShot

    private int _wave;
    private int _maxWave = 2;

    private float _randomPowerUp;

    private UIManager _uiManager;



    // Start is called before the first frame update
    void Start()
    {
        _wave = 0;
        _uiManager = GameObject.Find("UI_Manager").GetComponent<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        _wave = Mathf.Clamp(_wave, 0, _maxWave);
    }

    public void StartSpawning()
    {
        StartCoroutine(EnemySpawn());
        StartCoroutine(PowerupSpawn());
    }
    IEnumerator EnemySpawn()
    {
        if (_enemy != null)
        {

            while (_stopSpawning == false)
            {


                switch (_wave)
                {
                    case 0:
                        EnemySpawn(0);
                        yield return new WaitForSeconds(2);
                        break;
                    case 1:
                        EnemySpawn(1);
                        yield return new WaitForSeconds(2);
                        break;
                    case 2:
                        EnemySpawn(Random.Range(0, 2));
                        yield return new WaitForSeconds(1);
                        break;
                }
 
            }
        }
    }

    IEnumerator PowerupSpawn()
    {
        yield return new WaitForSeconds(3);

        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3, 7));
            _powerUpSpawnPos = new Vector3(Random.Range(-10.5f, 11f), 8, 0);
            _randomPowerUp = Random.Range(0, 101);

            if (_randomPowerUp <= 40)
            {
                int RandomPUP = _highChance[Random.Range(0, _highChance.Length)];
                Instantiate(_powerups[RandomPUP], _powerUpSpawnPos, Quaternion.identity);
            }
            else if (_randomPowerUp > 40 && _randomPowerUp <= 70)
            {
                int RandomPUP = _mediumChance[Random.Range(0, _mediumChance.Length)];
                Instantiate(_powerups[RandomPUP], _powerUpSpawnPos, Quaternion.identity);
            }
            else if (_randomPowerUp > 70 && _randomPowerUp <= 90)
            {
                int RandomPUP = _lowChance[Random.Range(0, _lowChance.Length)];
                Instantiate(_powerups[RandomPUP], _powerUpSpawnPos, Quaternion.identity);
            }
            else
            {
                int RandomPUP = _veryLowChance[Random.Range(0, _veryLowChance.Length)];
                Instantiate(_powerups[RandomPUP], _powerUpSpawnPos, Quaternion.identity);
            }

        }
        
    }

    public void PlayerDeath()
    {
        _stopSpawning = true;
    }

    public void NextWave()
    {
        _wave++;

        if (_wave > _maxWave)
        {
            _wave = _maxWave;
        }

        _uiManager.UpdateWaveCount(_wave);

    }

    public void EnemySpawn(int enemyID)
    {
        if (enemyID == 0)
        {
            _enemySpawnPos = new Vector3(Random.Range(-10.5f, 11f), 8, 0);
            GameObject newEnemy = Instantiate(_enemy[enemyID], _enemySpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }
        else if (enemyID == 1)
        {
            _enemySpawnPos = new Vector3(Random.Range(0, _siderSpawnPos.Length), 8, 0);
            GameObject newEnemy = Instantiate(_enemy[enemyID], _enemySpawnPos, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
        }

    }
}
