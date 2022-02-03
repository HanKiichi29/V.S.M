using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;

/// <summary>
/// 浮いている敵
/// </summary>

public class Beholder : AllEnmy
{
    //このモンスターの動ける範囲を変更できる
    [SerializeField] private float MoveDistance;

    //TはTweenのTです
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
