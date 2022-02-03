using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UnityEngine.SceneManagement;

/// <summary>
/// �^�C�g����ʂ̃A�j���[�V������V�[���J��
/// </summary>
public class TitleAnimation : MonoBehaviour
{

    [SerializeField] private GameObject Bloon;

    [SerializeField] private Text TitleText;
    [SerializeField] private Text GuideText;
    [SerializeField] private Image FadePanel;

    private const string SceneName = "Story";

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
        FadePanel.DOFade(1, 3).OnComplete(()=> { SceneManager.LoadScene(SceneName); });
    }


    //�e�L�X�g�̓_��
    private void TextBlinking()
    {
        GuideText.DOFade(0, 2.0f)
            .SetEase(Ease.Linear)
            .SetLink(gameObject)
            .SetLoops(-1, LoopType.Yoyo);
    }


}
