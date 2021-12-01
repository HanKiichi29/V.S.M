using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

  
    protected virtual void EnmyCameraUpdate()
    {
       

    }

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

    protected virtual void DeadEnmy()
    {
        Stop = false;
        EnmyAnimation.SetTrigger("Dead");
    }
}
