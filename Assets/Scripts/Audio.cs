using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    private GameObject _bossMusic;
    private GameObject _backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        _backgroundMusic = GameObject.Find("Background");
        _backgroundMusic.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
