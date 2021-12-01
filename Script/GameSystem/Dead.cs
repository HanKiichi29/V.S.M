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
    [SerializeField] private GameObject player;
    

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
        DeadSound.Play();

        DeadPanel.DOFade(1, 3)
            .OnComplete(() => { DeadSelect(); });
    }

    private void DeadSelect()
    {

        DeadText.DOText("Why did you kill?", 0.5f)
            .OnComplete(() => { BackSelect();  });



    }

    public void BackSelect()
    {
        DeadText.DOFade(0, 2)
            .SetLink(gameObject)
            .SetDelay(5)
            .OnComplete(() => {

                SceneManager.LoadScene("StageSelect"); 
            });

    }

}
