using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float Duration;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= Duration)
        {
            Destroy(gameObject);
        }
    }
}
