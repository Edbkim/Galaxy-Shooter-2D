using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _backgroundMusic;
    [SerializeField]
    private GameObject _bossMusic;
    private bool _isGameOver;


    private void Start()
    {
        
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void GameOver()
    {
        _isGameOver = true;
        _bossMusic = GameObject.Find("Boss_Music");
        _bossMusic.SetActive(false);
    }
}
