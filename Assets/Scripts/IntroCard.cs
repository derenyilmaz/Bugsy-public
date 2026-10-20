using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IntroCard : MonoBehaviour
{
    [SerializeField] private bool fadeIn;
    [SerializeField] private bool fadeOut;
    [SerializeField] private bool skippable;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private float durationInSeconds;
    [SerializeField] private float minSkipThreshold = 0.5f;

    private bool _skipped;
    private float _elapsedTime;

    private void Update()
    {
        if (_elapsedTime < minSkipThreshold) return;
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            Debug.Log("Pressed anyKey");
            _skipped = true;
        }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Debug.Log("Pressed leftButton");
            _skipped = true;
        }
    }

    public IEnumerator Process()
    {
        _elapsedTime = 0f;
        if (fadeIn) canvasGroup.DOFade(1, 1f);
        AudioManager.Instance.PlaySound(audioClip);
        
        while (true)
        {
            if (_elapsedTime >= durationInSeconds || (skippable && _skipped))
                break;

            yield return null;
            _elapsedTime += Time.deltaTime;
        }
        if (fadeOut) canvasGroup.DOFade(0, 1f);
    }
}

