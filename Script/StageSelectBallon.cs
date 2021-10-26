using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// ステージセレクトの気球の処理
/// </summary>

public class StageSelectBallon : MonoBehaviour
{

    

    [SerializeField] private GameObject Balloon;
    [SerializeField] private GameObject CorsePanel;

    [SerializeField] private Text CorseNameText;
    [SerializeField] private Image FadePanel;

    private int SelectNunmber;
    private int CorceDate=0;

    private float BalloomMoveTime;
    private AudioSource audioSource;

    bool Moveing;


    

    private void Start()
    {

        CorceDate = PlayerPrefs.GetInt("Corse");
        Moveing = true;


        if (CorceDate > 0)
        {
            CorceDate = 0;
        }

        StageNumber();
        FadeOut();


        audioSource=GetComponent<AudioSource>();
        
        BalloomMoveTime = 3.3f;

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.A) && Moveing == false)
            .ThrottleFirst(TimeSpan.FromSeconds(BalloomMoveTime))
            .Subscribe(_ => MoveLeft()).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.D) && Moveing == false)
            .ThrottleFirst(TimeSpan.FromSeconds(BalloomMoveTime))
            .Subscribe(_ => MoveRight()).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return)&&Moveing==false)
            .ThrottleFirst(TimeSpan.FromSeconds(BalloomMoveTime))
            .Subscribe(_ => Select()).AddTo(this);


    }


    private void FadeOut()
    {
        FadePanel.DOFade(0, 1.5f).OnComplete(() => { Moveing = false; })
            .SetLink(gameObject)
            .SetDelay(1.5f);
    }



    
    void MoveRight()
    {
        
        if (SelectNunmber >= CorceDate)
        {
            return;
        }

        
        CorsePanelUp();

        Balloon.transform.DOMoveX(70,2.5f).SetRelative()
            .SetLink(gameObject)
            .OnComplete(()=> { CorsePanelDown(); });
        SelectNunmber++;
   
    }

    void MoveLeft()
    {
        if (SelectNunmber <= 0)
        {
            return;
        }

        CorsePanelUp();

        Balloon.transform.DOMoveX(-70, 2.5f).SetRelative()
            .SetLink(gameObject)
            .OnComplete(() => {  CorsePanelDown(); });

        SelectNunmber--;
      
    }

    void Select()
    {
        Moveing = true;
        PlayerPrefs.SetInt("Corse", SelectNunmber);

        audioSource.DOFade(0, 1.5f);
        FadePanel.DOFade(1, 1.5f)
            .SetLink(gameObject)
            .OnComplete(() => 
            {
                string StageName = "Stage" + (SelectNunmber+1).ToString();
                SceneManager.LoadScene(StageName);
            })
        
       .SetDelay(1.5f);
       

    }


    void CorsePanelDown()
    {
      

        StageNumber();


        CorsePanel.transform.DOMoveY(-220, 0.1f).SetRelative()
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject)
            .OnComplete(() => { Moveing = false; });
        
        
            
    }

    void CorsePanelUp()
    {
        Moveing = true;
        
     
            CorsePanel.transform.DOMoveY(220, 0.1f).SetRelative()
                .SetEase(Ease.InOutSine)
                .SetLink(gameObject);
   
        
    }


    void StageNumber()
    {
        switch (SelectNunmber)
        {
            case 0:
                CorseNameText.text = "1-1";
                break;
            case 1:
                CorseNameText.text = "1-2";
                break;
            case 2:
                CorseNameText.text = "1-3";
                break;
            case 3:
                CorseNameText.text = "1-4";
                break;
        }
    }


}
