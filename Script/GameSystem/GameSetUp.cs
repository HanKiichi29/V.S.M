using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// ゲームの設定（BGMの音量調整やポーズ画面の表示など）
/// </summary>

public class GameSetUp : MonoBehaviour
{

    [SerializeField] private GameObject SetUpPanel;

    private bool Display = true;
    

    private AudioSource BgmBox;
    private void Start()
    {
        BgmBox = gameObject.GetComponent<AudioSource>();

        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            .Subscribe(_ =>Pause()).AddTo(this);

        
    }

    /// <summary>
    /// BGMの音量調整
    /// </summary>
    /// <param name="value"></param>
    public void VolumeChange(float value)
    {
        BgmBox.volume = value;
    }


    /// <summary>
    /// ポーズ画面の表示と非表示
    /// </summary>
    private void Pause()
    {
        if (Display)
        {
            Display = false;
            SetUpPanel.SetActive(true);
        }else if (!Display)
        {
            Display = true;
            SetUpPanel.SetActive(false);
        }
       
    }

    


}
