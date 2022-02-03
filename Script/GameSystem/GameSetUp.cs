using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

/// <summary>
/// �Q�[���̐ݒ�iBGM�̉��ʒ�����|�[�Y��ʂ̕\���Ȃǁj
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
    /// BGM�̉��ʒ���
    /// </summary>
    /// <param name="value"></param>
    public void VolumeChange(float value)
    {
        BgmBox.volume = value;
    }


    /// <summary>
    /// �|�[�Y��ʂ̕\���Ɣ�\��
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
