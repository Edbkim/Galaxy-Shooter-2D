using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private int _speed = 10;

    private bool _isEnemyLaser;

    private bool _isHomingLaser;

    private Transform _target;
    private Rigidbody2D _rigidBody;
    private float _rotateSpeed = 200f;
    private float _homingSpeed = 10;


    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemyLaser == false && _isHomingLaser == false)
        {
            MoveUp();
        }
        else if (_isHomingLaser == true)
        {
            _target = GameObject.FindGameObjectWithTag("Enemy").transform;
            HomingLaser();
            Destroy(this.gameObject, 2);
        }      
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 7.5f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5.3f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }



    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            Debug.Log("Got Hit");
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignHomingLaser()
    {
        _isHomingLaser = true;
    }

    void HomingLaser()
    {

        Vector2 direction = (Vector2)_target.position - _rigidBody.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;
        _rigidBody.angularVelocity = -rotateAmount * _rotateSpeed;
        _rigidBody.velocity = transform.up * _homingSpeed;

    }
}
