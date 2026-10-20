using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCardController : MonoBehaviour
{
    [SerializeField] private List<IntroCard> introCards;
    [SerializeField] private bool loadBossSceneNext;

    private void Start()
    {
        StartCoroutine(ProcessIntroCards());
    }

    private IEnumerator ProcessIntroCards()
    {
        foreach (var introCardItem in introCards)
        {
            introCardItem.gameObject.SetActive(true);
            yield return introCardItem.Process();
            introCardItem.gameObject.SetActive(false);
        }

        if (loadBossSceneNext) SceneManager.LoadScene("Scenes/BossScene");
        else SceneManager.LoadScene("Scenes/MenuScene");
    }
}


