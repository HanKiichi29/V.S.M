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
/// ストーリー再生
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

        //ストーリーを流す
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return)&& isStory)
            .Subscribe(_ => StoryBeginning()).AddTo(this);

        //ストーリーをスキップする
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.S))
            .Subscribe(_ => StorySkip()).AddTo(this);

    }


    /// <summary>
    /// csvを読み込む
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
    /// ストーリーを流す
    /// </summary>
    void StoryBeginning()
    {
        isStory = false;

        if (CsvCount<=(CsvDate.Count-1))
        {
            StoryText.text = " ";


            //テキストを点滅させる
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

        //チェックを入れるとさいごのスタッフロールのときの処理になる（タイトルに戻る）
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
