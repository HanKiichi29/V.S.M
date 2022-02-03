using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// �㉺�����E�ɓ����A�j���[�V�����̏���
/// </summary>
public class OrnamentAnimation : MonoBehaviour
{

    /// <summary>
    /// ��������
    /// </summary>
    [SerializeField] private float MoveDistans;
    /// <summary>
    /// ���������܂łɍs���b��
    /// </summary>
    [SerializeField] private float MoveTime;
    /// <summary>
    /// EaseType
    /// </summary>
    [SerializeField] private Ease EaseTypes;
    /// <summary>
    /// true�ɂ��邱�Ƃŉ��ړ��ɂȂ�
    /// </summary>
    [SerializeField,Header("�`�F�b�N������Ɖ��ړ��ɂȂ�"),Space(10)] private bool Change;
   

    private void Start()
    {
        MoveAnimation();
    }

   
    private void MoveAnimation()
    {
        if (Change)
        {
            this.gameObject.transform.DOLocalMoveX(MoveDistans, MoveTime)
          .SetLink(gameObject)
          .SetEase(EaseTypes)
          .SetLoops(-1, LoopType.Yoyo);

        }
        else if(!Change)
        {
            this.gameObject.transform.DOLocalMoveY(MoveDistans, MoveTime)
          .SetLink(gameObject)
          .SetEase(EaseTypes)
          .SetLoops(-1, LoopType.Yoyo);
        }

     

    }

}
