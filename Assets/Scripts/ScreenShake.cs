using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{

    private Transform _transform;
    private float _shakeDuration = 0f;
    private float _shakeMagnitude = 0.1f;
    private float _dampingSpeed = 1f;
    Vector3 _initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_shakeDuration > 0)
        {
            transform.localPosition = _initialPosition + Random.insideUnitSphere * _shakeMagnitude;
            _shakeDuration -= Time.deltaTime * _dampingSpeed;
        }
        else
        {
            _shakeDuration = 0f;
            transform.localPosition = _initialPosition;
        }
    }

    void OnEnable()
    {
        _initialPosition = transform.localPosition;
    }

    public void TriggerShake()
    {
        _shakeDuration = 1.0f;
    }
}
