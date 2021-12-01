using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using DG.Tweening;


public class Slime : AllEnmy
{

    private Tween T_SlimeMove;

    [SerializeField] private float MoveDistance = 5;
  


    private void Start()
    {
        EnmyAnimation = GetComponent<Animator>();
        EnmySpeed = 4;

        SlimeMove();
     
    }

    private void SlimeMove()
    {
        T_SlimeMove = transform.DOMoveX(transform.position.x + MoveDistance, EnmySpeed)
                       .SetEase(Ease.Linear)
                       .SetLink(gameObject)
                       .SetLoops(-1, LoopType.Yoyo)
                       .OnStepComplete(() =>
                       {
                           transform.DORotate(new Vector3(0, transform.eulerAngles.y + 180, 0), 1)
                           .OnComplete(() => T_SlimeMove.Play());
                           T_SlimeMove.Pause();
                       });
        T_SlimeMove.Play();

    }


   

    protected override void EnmyCameraUpdate()
    {
        base.EnmyCameraUpdate();

        if (!Stop)
        {
            T_SlimeMove.Pause();
        }


        //カメラに映ってるときに動く
        if (Rendered)
        {
            if (Stop)
            {
                T_SlimeMove.Play();
            }


            /* if (Vector3.Distance(transform.position, PlayerObj.transform.position) <= 4)
             {
                // Debug.Log("プレイヤーの前にいます");
                 playerf = false;
             }
            */

        }

    }

    //Playerによって倒された時
   protected override void DeadEnmy()
   {
       base.DeadEnmy();
   }

}
