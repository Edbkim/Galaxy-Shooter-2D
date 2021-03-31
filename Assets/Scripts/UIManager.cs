﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Text _gameOver;
    [SerializeField]
    private Text _restart;

    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Text _ammoMaxText;

    private int _currentScore;
    private int _maxAmmo;

    [SerializeField]
    private Image _livesImage;
    [SerializeField]
    private Sprite[] _livesSprites;



    // Start is called before the first frame update
    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }


    public void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    public void UpdateLives(int currentLives)
    {
        //display image sprite
        //give it a new one based on the index
        _livesImage.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }

    }

    public void UpdateAmmo(int ammo)
    {
        if (ammo >= _maxAmmo)
        {
            _ammoText.color = new Color32(136, 255, 249, 255);
            _ammoText.text = "Ammo: " + ammo;
        }
        else if (ammo >= 1 && ammo < _maxAmmo)
        {
            _ammoText.color = Color.white;
            _ammoText.text = "Ammo: " + ammo;
        }
        else if (ammo <= 0)
        {
            _ammoText.text = "OUT OF AMMO";
            _ammoText.color = Color.yellow;
        }
    }

    public void UpdateMaxAmmo(int maxAmmo)
    {
        _ammoMaxText.text = "Max Ammo: " + maxAmmo;
        _maxAmmo = maxAmmo;
    }
    void GameOverSequence()
    {
        _restart.gameObject.SetActive(true);
        StartCoroutine(GameOver());
    }

    IEnumerator GameOver()
    {
        while (true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(.5f);
            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(.5f);
        }

    }



}
