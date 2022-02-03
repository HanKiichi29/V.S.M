using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UniRx;
using UnityEngine.SceneManagement;

/// <summary>
/// ステージクリア
/// </summary>
public class Clear : MonoBehaviour
{

    [SerializeField] private Text ClearText;
    [SerializeField] private Text GuideText;


    [SerializeField] private Image ClearFadePanel;
    [SerializeField] private GameObject Player;

    private float FadeSpeed;
    private PlayerController controller;

    private bool ClearStop=true;
    private bool Click=false;


    private void Start()
    {
        FadeSpeed = 5.5f;

        GameStaet();
      

        var P = GameObject.FindGameObjectWithTag("Player");
        Player = P;
        controller = P.GetComponent<PlayerController>();

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return)&&Click==true)
            .Subscribe(_ => BackSelect()).AddTo(this);
    }

    /// <summary>
    /// ゲーム開始時のフェイドアウト
    /// </summary>
    public void GameStaet()
    {
        ClearFadePanel.DOFade(0, 0.5f).SetLink(gameObject);
    }

    //ゴールした時の処理
    private void OnTriggerStay(Collider other)
    {
        if (other.name == Player.name)
        {
            if (Input.GetKey(KeyCode.W)&&ClearStop)
            {
                StageClear();
            }

        }
    }


    /// <summary>
    /// ステージクリアしたときの処理
    /// </summary>
    private void StageClear()
    {
        ClearStop = false;

        int CorseDate = PlayerPrefs.GetInt("Corse");
        CorseDate++;
        
        PlayerPrefs.SetInt("Course", CorseDate);

        controller.Clear();
        ClearFadePanel.DOFade(1, FadeSpeed).OnComplete(()=> { ClearSelect(); });

    }

    private void ClearSelect()
    {
        ClearText.DOText("Stage Clear", 1.0f).OnStepComplete(() =>
        {
            GuideText.DOFade(1, 1).OnComplete(() => { Click = true; });
        });


    }

    public void BackSelect()
    {

        SceneManager.LoadScene("StageSelect");

    }
    

}
