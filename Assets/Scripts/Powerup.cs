﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private int _speed = 3;
    private int _suckSpeed = 5;

    [SerializeField]
    private int _ammoDown = 1;

    [SerializeField] //0 Triple / 1 Speed / 2 Shield
    private int _powerupID;

    [SerializeField]
    private AudioClip _powerUPSFX;

    private Transform _target;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
        
        if (Input.GetKey(KeyCode.C) && transform.position.y <= 6 && transform.position.y >= -3)
        {
            float step = _suckSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, _target.position, step);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {

            Player player = other.gameObject.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_powerUPSFX, transform.position);

            if (player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        Debug.Log("Speed up!");
                        player.SpeedUp();
                        break;
                    case 2:
                        Debug.Log("Shields!");
                        player.ShieldActive();
                        break;
                    case 3:
                        Debug.Log("Reloaded Ammo!");
                        player.Reload();
                        break;
                    case 4:
                        Debug.Log("Health Restored");
                        player.Heal();
                        break;
                    case 5:
                        player.HomingShotActive();
                        break;
                    case 6:
                        player.AmmoMaxDown(_ammoDown);
                        break;
                    default:
                        break;
                }

            }

            Destroy(this.gameObject);
        }
    }

}
