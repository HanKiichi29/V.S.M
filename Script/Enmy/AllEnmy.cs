using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// すべての敵の共通の処理
/// </summary>
public class AllEnmy : MonoBehaviour
{

    #region 変数

    private const string MainCamera = "MainCamera";
    private const string PlayerTag = "Player";

    protected Animator EnmyAnimation;
    protected Transform PlayerPosition;

    protected bool Rendered=false;
    protected bool Stop=true;


    #region 敵のステータス

   [SerializeField]protected float EnmySpeed;
   
    #endregion


    #endregion

    //全ての敵のアニメーション状態
    public enum EnmyStatus
    {
        Idle,
        Dead,
        Work
    }

  
    /// <summary>
    /// 画面に映ったか判定する
    /// </summary>
    protected virtual void EnmyCameraUpdate()
    {
       

    }

    /// <summary>
    /// 画面から見えなくなったら動きを止める
    /// </summary>
    private void OnWillRenderObject()
    {
        if (Camera.current.tag == MainCamera)
        {
            Rendered = true;
        }
    }

    private void LateUpdate()
    {
        Rendered = false;
    }


    /// <summary>
    /// プレイヤーに当たった時の処理
    /// </summary>
    /// <param name="collision"></param>
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == PlayerTag)
        {

            DeadEnmy();
            
        }
    }



    private void Update()
    {

        EnmyCameraUpdate();

    }

    /// <summary>
    /// 敵キャラクターが死亡した時のアニメーション
    /// </summary>
    protected virtual void DeadEnmy()
    {
        Stop = false;
        EnmyAnimation.SetTrigger("Dead");
    }
}
