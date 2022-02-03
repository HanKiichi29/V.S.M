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
    [SerializeField] private GameObject CoursePanel;

    [SerializeField] private Text CourseNameText;
    [SerializeField] private Image FadePanel;

    [SerializeField] private AudioClip ClickSE;

    private static int SelectNunmber;
    private static int CourseDate = 0;
    private static int SelectCourse;
    private float BalloomMoveTime;

    private AudioSource audioSource;

    //気球の初期位置
    private static Vector3 NowPosition = new Vector3(-100, 8, 0);

    public bool Moving;

    private void Awake()
    {
        
    }
    private void Start()
    {

        CourseDate = PlayerPrefs.GetInt("Course");
        Moving = true;

        StageNumber();
        FadeOut();

        Balloon.transform.position = NowPosition;


        audioSource=GetComponent<AudioSource>();
        
        BalloomMoveTime = 3.3f;

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.A) && Moving == false)
            .ThrottleFirst(TimeSpan.FromSeconds(BalloomMoveTime))
            .Subscribe(_ => MoveLeft()).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.D) && Moving == false)
            .ThrottleFirst(TimeSpan.FromSeconds(BalloomMoveTime))
            .Subscribe(_ => MoveRight()).AddTo(this);

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return)&&Moving==false)
            .ThrottleFirst(TimeSpan.FromSeconds(BalloomMoveTime))
            .Subscribe(_ => Select()).AddTo(this);


        if (CourseDate >= SelectCourse)
        {
            SelectCourse = CourseDate;
        }

    }


    private void FadeOut()
    {
        FadePanel.DOFade(0, 1.5f).OnComplete(() => { Moving = false; })
            .SetLink(gameObject)
            .SetDelay(1.5f);
    }



    
    void MoveRight()
    {
        
        if (SelectNunmber >= SelectCourse)
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
        Moving = true;

        PlayerPrefs.SetInt("Course", SelectNunmber);
        NowPosition = Balloon.transform.position;

        AudioSetting();

        FadePanel.DOFade(1, 1.5f)
            .SetLink(gameObject)
            .OnComplete(() => 
            {
                if (5 <= SelectNunmber + 1)
                {
                    SceneManager.LoadScene("Staffroll");
                }
                else
                {
                    string StageName = "Stage" + (SelectNunmber + 1).ToString();
                    SceneManager.LoadScene(StageName);
                }
               
            })
        
       .SetDelay(1.5f);
       

    }

    void AudioSetting()
    {
        audioSource.DOFade(0, 1.5f);
        audioSource.PlayOneShot(ClickSE);
    }


    void CorsePanelDown()
    {
      

        StageNumber();


        CoursePanel.transform.DOMoveY(-220, 0.1f).SetRelative()
            .SetEase(Ease.Linear)
            .SetLink(gameObject)
            .OnComplete(() => { Moving = false; });
        
        
            
    }

    void CorsePanelUp()
    {
        Moving = true;


        CoursePanel.transform.DOMoveY(220, 0.1f)
            .SetRelative()
            .SetEase(Ease.InOutSine)
            .SetLink(gameObject);
   
        
    }


    void StageNumber()
    {
        
        //４ステージしかないため本来ステージ5以上になると別のテキストを表示する
        if (5 <= SelectNunmber + 1)
        {
            CourseNameText.text = "Staffroll";
           // Debug.Log("ステージ5以上はないよ");
        }
        else
        {
            CourseNameText.text = "1-" + (SelectNunmber + 1);
        }
        
    }


}
