using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[RequireComponent(typeof(AudioSource))]
public class FadeOutController : MonoBehaviour
{
    AudioSource _source;

    [SerializeField]
    float Delay, FadeOut;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        LeanTween.value(gameObject,
            f =>
            {
                _source.volume = f;
            }, _source.volume, 0f, FadeOut)
            .setDelay(Delay);
    }
}
