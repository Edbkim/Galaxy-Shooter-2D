using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private GameObject _laser;

    private Vector3 _offset;


    [SerializeField]
    private int _speed = 5;

    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    private int _lives = 3;

    // Start is called before the first frame update
    void Start()
    {
        // take the current position and set = new position 0 0 0
        transform.position = new Vector3(0, 0, 0);

        //bill is 40
        //tip is 20% or based on what the user wants
        //calculate total amount
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();

        // if i hit space key
        //spawn the gameobject


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

        transform.Translate(direction * _speed * Time.deltaTime);


        //if player position on the y is greater than 0
        //y position = 0

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

        _offset = new Vector3(0, 0.8f, 0);

        _canFire = Time.time + _fireRate;
            Instantiate(_laser, transform.position + _offset, Quaternion.identity);

    }

    public void Damage()
    {
        _lives--;
        
        if (_lives < 1)
        {
            Destroy(this.gameObject);
        }
    }
}
