using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[RequireComponent(typeof(AudioSource))]
public class DelayController : MonoBehaviour
{
    AudioSource _source;

    [SerializeField]
    float Delay, FadeTime;

    float timer = 0f;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
    }


    void Update()
    {
        if (timer > Delay)
        {
            _source.enabled = true;
            LeanTween.value(gameObject,
                f =>
                {
                    _source.volume = f;
                },
                0f, _source.volume, FadeTime);

            this.enabled = false;
        }

        timer += Time.unscaledDeltaTime;
    }
}
