using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemy;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] _powerups;

    [SerializeField]
    private bool _stopSpawning;

    private Vector3 _enemySpawnPos;
    private Vector3 _powerUpSpawnPos;

    private float _randomPowerUp;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PowerupSpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            yield return new WaitForSeconds(3);

            while (_stopSpawning == false)
            {
                _enemySpawnPos = new Vector3(Random.Range(-10.5f, 11f), 8, 0);
                GameObject newEnemy = Instantiate(_enemy, _enemySpawnPos, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;

                yield return new WaitForSeconds(2);
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
            _randomPowerUp = Random.value;

            if (_randomPowerUp <= 0.8)
            {
                int RandomPUP = Random.Range(0, 5);
                Instantiate(_powerups[RandomPUP], _powerUpSpawnPos, Quaternion.identity);
            }
            else if (_randomPowerUp > 0.8)
            {
                Instantiate(_powerups[5], _powerUpSpawnPos, Quaternion.identity);
            }

        }
        
    }

    public void PlayerDeath()
    {
        _stopSpawning = true;
    }
}
