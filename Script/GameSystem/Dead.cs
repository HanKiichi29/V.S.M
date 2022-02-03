using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// ゲームオーバー
/// </summary>
public class Dead : MonoBehaviour
{
    [SerializeField] private Image DeadPanel;
    [SerializeField] private Text DeadText;

    private GameObject player;  
    private AudioSource DeadSound;
    private PlayerController controller;

    private void Start()
    {
        DeadSound = GetComponent<AudioSource>();

        var P = GameObject.FindGameObjectWithTag("Player");
        player = P;
        controller = P.GetComponent<PlayerController>();
    }



    public void DeadEnmy()
    {
        controller.Move = false;

        DeadPanel.DOFade(1, 0.5f);

        //メインBGMを消して死亡時の効果音を鳴らす
        DeadSound.DOFade(0, 2);

        DeadText.DOFade(1, 1)
           .OnComplete(() => { BackSelect(); });

    }

   

    public void BackSelect()
    {
        DeadText.DOFade(0, 2)
            .SetLink(gameObject)
            .SetDelay(3)
            .OnComplete(() => {

                SceneManager.LoadScene("StageSelect"); 
            });

    }

}
