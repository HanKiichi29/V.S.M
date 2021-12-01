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
/// ストーリー
/// </summary>

public class Story : MonoBehaviour
{
    [SerializeField] private TextAsset CsvFile;

    List<string[]> CsvDate = new List<string[]>();

    [SerializeField] private Text StoryText;
    [SerializeField] private float NovelSpeed;


    [SerializeField] private Image FadePanel;


    private bool isStory = true;
    private int CsvCount = 0;


    private void Start()
    {
        DownloadCsv();

      

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return)&& isStory)
            .Subscribe(_ => StoryBeginning()).AddTo(this);

    }


    private void DownloadCsv()
    {

        StringReader reader = new StringReader(CsvFile.text);

        while (reader.Peek() != -1)
        {
            string liner = reader.ReadLine();
            CsvDate.Add(liner.Split(' '));
        }

    }

    void StoryBeginning()
    {
        isStory = false;

        if (CsvCount<=CsvDate.Count-1)
        {
            StoryText.text = " ";

            StoryText.DOText(CsvDate[CsvCount][0], NovelSpeed)
                .SetLink(gameObject)
                .SetEase(Ease.Linear)
                .OnComplete(() => {
                    isStory = true;
                    });

            CsvCount++;
        }
       

        
    }

   

}
