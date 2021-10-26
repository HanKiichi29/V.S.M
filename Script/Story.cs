using System.Collections;
using System.Collections.Generic;
using System;
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
    [SerializeField] private Text StoryText;
    [SerializeField] private Image FadePanel;

    [SerializeField] private List<string> Dialogue = new List<string>();

    private int TextNumber;
    private int DialogueNumber;

    private void Start()
    {
        StoryText=GetComponent<Text>();

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return))
            .Subscribe(_ => StoryBeginning()).AddTo(this);

    }

    void StoryBeginning()
    {

        StoryText.DOText("ひなたバカ", 0.5f);

    }

    

}
