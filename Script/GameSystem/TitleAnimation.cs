using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UnityEngine.SceneManagement;

public class TitleAnimation : MonoBehaviour
{

    [SerializeField] private GameObject Bloon;

    [SerializeField] private Text TitleText;
    [SerializeField] private Text GuideText;
    [SerializeField] private Image FadePanel;


    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();


        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return))
            .ThrottleFirst(TimeSpan.FromSeconds(10))
            .Subscribe(_ => GoSelect()).AddTo(this);

        TextBlinking();

    }

    private void GoSelect()
    {
        audioSource.DOFade(0, 2.5f);
        FadePanel.DOFade(1, 3).OnComplete(()=> { SceneManager.LoadScene("StageSelect"); });
    }

    private void TextBlinking()
    {
        GuideText.DOFade(0, 2.0f)
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject)
            .SetLoops(-1, LoopType.Yoyo);
    }


}
