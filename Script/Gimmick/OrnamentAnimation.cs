using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 上下か左右に動くアニメーションの処理
/// </summary>
public class OrnamentAnimation : MonoBehaviour
{

    /// <summary>
    /// 動く距離
    /// </summary>
    [SerializeField] private float MoveDistans;
    /// <summary>
    /// 動く距離までに行く秒数
    /// </summary>
    [SerializeField] private float MoveTime;
    /// <summary>
    /// EaseType
    /// </summary>
    [SerializeField] private Ease EaseTypes;
    /// <summary>
    /// trueにすることで横移動になる
    /// </summary>
    [SerializeField,Header("チェックをつけると横移動になる"),Space(10)] private bool Change;
   

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
