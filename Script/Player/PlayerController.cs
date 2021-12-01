using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


/// <summary>
/// プレイヤーの処理
/// </summary>
public class PlayerController : MonoBehaviour
{
    #region 変数


    //プレイヤージャンプの高さ
    [SerializeField]private float JumpHeight;

    //プレイヤーの速度
    [SerializeField]private float PlayerSpeed;

    //プレイヤーの速度制限
    [SerializeField] private float PlayerMaxSpeed;

    //魔法攻撃
    [SerializeField] private int MagicCoolTime;

    [SerializeField] private Text MagicCoolTimeText;

    [SerializeField] private Dead dead;

   
    

    //private関数//

    //プレイヤーの操作をとめる
    public bool Move=true;

    //地面のフラッグ
    private bool Groundflag=false;

    //プレイヤーのアニメーション
    private Animator animator;

    //重力
    private float GravityScael;

   //string//
    private const string GroundName = "Ground";
    private const string BalloonName = "Balloon";
    private const string EnmyName = "Ground";

    Rigidbody PlayerGrvity;
    Vector3 StartPosition;
    bool forward = true;
    #endregion

    void Start()
    {

        Application.targetFrameRate =60;


        //プレイヤーのRigidbodyの取得
        PlayerGrvity = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        StartPosition = transform.position;

        //プレイヤー右に移動
        Observable.EveryUpdate()
            .Where(_=>(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && Move == true))
            .Subscribe(_ => 
            {
                forward = true;
                PlayerMove();
            })
            .AddTo(this);

        //プレイヤー左に移動
        Observable.EveryUpdate()
            .Where(_=>(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && Move==true))
            .Subscribe(_ =>
            {
                forward = false;
                PlayerMove();
            })
            .AddTo(this);

        //Playerのジャンプ
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Space) && Move == true)
            .Subscribe(_ => Jump()).AddTo(this);


        //テレポートの魔法
        Observable.EveryUpdate()
            .Where(_ => Input.GetKeyDown(KeyCode.Return) && Move == true)
            .ThrottleFirst(TimeSpan.FromSeconds(MagicCoolTime))
            .Subscribe(_ =>Teleport()).AddTo(this);


        Observable.EveryUpdate()
            .Subscribe(_ =>PlayerGrvity.AddForce(new Vector3(0,-20.8f,0),ForceMode.Acceleration)).AddTo(this);

        //プレイヤーの歩くアニメーション
        Observable.EveryUpdate()
            .Subscribe(_ => WalkAnimation()).AddTo(this);


    }

   
    
    private void WalkAnimation()
    {
        if (PlayerGrvity.velocity.magnitude > 0.1f)
        {
            animator.SetFloat("Walk", PlayerGrvity.velocity.magnitude);
        }
        else
        {
            animator.SetFloat("Walk", 0);
        }
    }

   

    private void PlayerMove()
    {

        var h = Input.GetAxis("Horizontal");
      
        //速度制限
        if (PlayerGrvity.velocity.magnitude < PlayerMaxSpeed)
        {
            PlayerGrvity.AddForce(h * PlayerSpeed, 0, 0);
        }

        PlayerLook();
    }

    void PlayerLook()
    {
        if (forward)
        {
            transform.rotation = Quaternion.LookRotation(-Vector3.left);

        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }

    }




    private void Jump()
    {
        if (Groundflag)
        {
            PlayerGrvity.AddForce(new Vector3(0, JumpHeight, 0));
        }
    }



    public void Clear()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Move = false;
        animator.SetTrigger("Clear");
    }

   


    //魔法での瞬間移動
    private void Teleport()
    {

         float MoveDistance = 6.5f;
         Ray ray = new Ray(transform.position,transform.forward);
         RaycastHit hit;
         if(Physics.Raycast(ray,out hit,MoveDistance))
         {
             if(!forward)
             {
                 transform.position = hit.point + new Vector3(transform.localScale.x, 0, 0);
             }
             else
             {
                 transform.position = hit.point - new Vector3(transform.localScale.x, 0, 0);
             }
             return;
         }

         transform.position += transform.forward * MoveDistance;

         
        
    }


    //瞬間移動を使った時のクールタイムを表示する
    private void CooltimeDisplay()
    {


        
    }

    #region 当たり判定処理
     
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == GroundName)
        {
            Groundflag = true;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == EnmyName)
        {
           dead.DeadEnmy();
        }

        if (transform.parent == null && collision.gameObject.tag == BalloonName)
        {
            PlayerGrvity.velocity = Vector3.zero;

            //180度回転させ前を向けさせる
            this.transform.rotation = Quaternion.Euler(0, 180, 0);

            var playerobject = new GameObject();
            playerobject.transform.parent = collision.gameObject.transform;
            transform.parent = playerobject.transform;
        }

        if (collision.gameObject.tag == "FallGround")
        {
            transform.position = StartPosition;
        }
    }



    private void OnCollisionExit(Collision collision)
    {

        if (collision.gameObject.tag == GroundName)
        {
            Groundflag = false;
        }

        if (transform.parent != null && collision.gameObject.tag == BalloonName)
        {

            Move = true;
            transform.parent = null;
            this.transform.rotation = Quaternion.Euler(0, 90, 0);

        }

    }

    #endregion

  

}
