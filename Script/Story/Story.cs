using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;
using DG.Tweening;

/// <summary>
/// �X�g�[���[�Đ�
/// </summary>

public class Story : MonoBehaviour
{
    [SerializeField] private TextAsset CsvFile;

    List<string[]> CsvDate = new List<string[]>();

    [SerializeField] private Text StoryText;
    [SerializeField] private Text GuidText;
    [SerializeField] private float NovelSpeed;


    [SerializeField] private Image FadePanel;
    [SerializeField] private bool End;

    private const string SceneName = "StageSelect";
    private const string TitleName = "Title";

    private bool isStory = false;
    private int CsvCount = 0;




    private void Start()
    {
        DownloadCsv();

        FadePanel.DOFade(0, 1)
            .SetLink(gameObject)
            .SetEase(Ease.Linear)
            .OnComplete(() => { 
                isStory = true;
            });

        //�X�g�[���[�𗬂�
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return)&& isStory)
            .Subscribe(_ => StoryBeginning()).AddTo(this);

        //�X�g�[���[���X�L�b�v����
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => StorySkip()).AddTo(this);

    }


    /// <summary>
    /// csv��ǂݍ���
    /// </summary>
    private void DownloadCsv()
    {

        StringReader reader = new StringReader(CsvFile.text);

        while (reader.Peek() != -1)
        {
            string liner = reader.ReadLine();
            CsvDate.Add(liner.Split(' '));
        }

    }

    /// <summary>
    /// �X�g�[���[�𗬂�
    /// </summary>
    void StoryBeginning()
    {
        isStory = false;

        if (CsvCount<=(CsvDate.Count-1))
        {
            StoryText.text = " ";


            //�e�L�X�g��_�ł�����
            GuidText.DOFade(0, 0.3f)
                .SetLink(gameObject)
                .SetEase(Ease.Linear)
                .SetLoops(2,LoopType.Yoyo);
                

            StoryText.DOText(CsvDate[CsvCount][0], NovelSpeed)
                .SetLink(gameObject)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    isStory = true;
                    });

            CsvCount++;

        }
        else
        {
            FadeIn();
        }
        
    }

    private void FadeIn()
    {

        isStory = false;

        //�`�F�b�N������Ƃ������̃X�^�b�t���[���̂Ƃ��̏����ɂȂ�i�^�C�g���ɖ߂�j
        if (End)
        {
            FadePanel.DOFade(1, 1)
         .SetLink(gameObject)
         .SetEase(Ease.Linear)
         .OnComplete(() => {
             SceneManager.LoadScene(TitleName);
         });
        }
        else
        {
            FadePanel.DOFade(1, 1)
          .SetLink(gameObject)
          .SetEase(Ease.Linear)
          .OnComplete(() => {
              SceneManager.LoadScene(SceneName);
          });
        }

       


    }

    private void StorySkip()
    {

        FadeIn();
    }


}
