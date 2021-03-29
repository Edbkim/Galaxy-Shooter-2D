using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private int _rotateSpeed = 15;

    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;



    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

    }

    // Update is called once per frame
    void Update()
    {
        //rotate object on the z axis 
        transform.Rotate(new Vector3 (0, 0, 1) *_rotateSpeed * Time.deltaTime);

    }

    //check for LASER collision (trigger)
    //instantiate explosion at position of the asteroid.

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_explosion.gameObject, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 1.3f);
        }
    }
}
