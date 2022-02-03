using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

/// <summary>
/// �����Ă���G
/// </summary>

public class Beholder : AllEnmy
{
    //���̃����X�^�[�̓�����͈͂�ύX�ł���
    [SerializeField] private float MoveDistance;

    //T��Tween��T�ł�
    private Tween T_BeholderMove;

    private void Start()
    {
        EnmySpeed = 2.0f;

        EnmyAnimation = GetComponent<Animator>();

        T_BeholderMove = transform.DOMoveY(MoveDistance, EnmySpeed)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetRelative()
                 .SetLink(gameObject)
                 .SetEase(Ease.Linear);
    }


    protected override void EnmyCameraUpdate()
    {
        base.EnmyCameraUpdate();

        if (!Stop)
        {
            T_BeholderMove.Pause();
        }

    }

    protected override void DeadEnmy()
    {
        base.DeadEnmy();
        
    }
}
