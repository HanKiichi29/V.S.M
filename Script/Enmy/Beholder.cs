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

    [SerializeField] private float MoveDistance;


    private void Start()
    {
        EnmySpeed = 2.0f;

        EnmyAnimation = GetComponent<Animator>();

        this.transform.DOMoveY(MoveDistance, EnmySpeed)
                .SetLoops(-1, LoopType.Yoyo)
                .SetRelative()
                .SetLink(gameObject)
                .SetEase(Ease.InOutSine);
    }


    protected override void EnmyCameraUpdate()
    {
        base.EnmyCameraUpdate();

    }

    protected override void DeadEnmy()
    {
        base.DeadEnmy();
        
    }
}
