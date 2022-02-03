using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

/// <summary>
/// プレイヤーの処理
/// </summary>

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerController : MonoBehaviour
{
    #region 変数


    //プレイヤージャンプの高さ
    [SerializeField]private float JumpHeight;

    //プレイヤーの速度
    [SerializeField]private float PlayerSpeed;

    //プレイヤーの速度制限
    [SerializeField] private float PlayerMaxSpeed;

    //魔法のクールタイム
    [SerializeField] private float CoolTime;
    //このDはDisplyのDです
    private float D_Colltime;
    //魔法のクールタイムを表示するテキスト
    [SerializeField] private Text MagicCoolTimeText;

    [SerializeField] private Dead dead;


    //プレイヤーの操作をとめる
    public bool Move = true;

    //private関数//


    //地面のフラッグ
    private bool Groundflag = false;

    private bool Forward = true;

    private bool AnimStop = true;

    private bool Timer = false;

    //プレイヤーのアニメーション
    private Animator animator;
    //AudioSourec
    private AudioSource PlayerAudio;

    //重力
    private float GravityScale = -20.8f;
    Rigidbody PlayerGrvity;

    #region TagName
    //string//
    private const string GroundName = "Ground";
    private const string BalloonName = "Balloon";
    private const string EnmyName = "Enmy";
    private const string WallName = "MagicWall";

    #endregion

    #endregion

    void Start()
    {
       
        ResetCoolTime();

        //プレイヤーのRigidbodyの取得
        PlayerGrvity = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        PlayerAudio = GetComponent<AudioSource>();

       // StartPosition = transform.position;

        //プレイヤー右に移動
        Observable.EveryUpdate()
            .Where(_=>(Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && Move == true))
            .Subscribe(_ => 
            {
                Forward = true;
                PlayerMove();
            })
            .AddTo(this);

        //プレイヤー左に移動
        Observable.EveryUpdate()
            .Where(_=>(Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D) && Move==true))
            .Subscribe(_ =>
            {
                Forward = false;
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
            .ThrottleFirst(TimeSpan.FromSeconds(CoolTime))
            .Subscribe(_ =>Teleport()).AddTo(this);


        Observable.EveryUpdate()
            .Subscribe(_ => PlayerGrvity.AddForce(new Vector3(0, GravityScale, 0), ForceMode.Acceleration)).AddTo(this);

        //プレイヤーの歩くアニメーション
        Observable.EveryUpdate()
            .Where(_=>AnimStop)
            .Subscribe(_ => WalkAnimation()).AddTo(this);

        //魔法を使った時のクールタイムの表示
        Observable.EveryUpdate()
            .Where(_ => Timer)
            .Subscribe(_ => CooltimeDisplay()).AddTo(this);
            


    }

   
    /// <summary>
    /// 動いているときのアニメーション
    /// </summary>
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

   
    /// <summary>
    /// プレイヤーの移動処理。速度制限付き
    /// </summary>
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

    /// <summary>
    /// プレイヤーが向いている方向に回転する
    /// </summary>
    void PlayerLook()
    {
        if (Forward)
        {
            transform.rotation = Quaternion.LookRotation(-Vector3.left);

        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.left);
        }

    }

    /// <summary>
    /// ジャンプ。至って普通のジャンプ
    /// </summary>
    private void Jump()
    {
        if (Groundflag)
        {
            PlayerGrvity.AddForce(new Vector3(0, JumpHeight, 0));
        }
    }


    /// <summary>
    /// クリアした時のアニメーション再生
    /// </summary>
    public void Clear()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        Move = false;
        animator.SetTrigger("Clear");
    }
   

    /// <summary>
    /// 特定の壁をすり抜けることができる魔法
    /// </summary>
    private void Teleport()
    {

        Timer = true;
        PlayerAudio.PlayOneShot(PlayerAudio.clip);

        float MoveDistance = 6.5f;
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, MoveDistance))
        {
            if (hit.collider.gameObject.tag == WallName)
            {
                transform.position += transform.forward * MoveDistance;
                return;
            }
            if (!Forward)
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

        if (D_Colltime >=0)
        {
            D_Colltime -= Time.deltaTime;
            MagicCoolTimeText.text = D_Colltime.ToString("F0");
        }
        else
        {
            ResetCoolTime();
            Timer = false;
            
        }
        
    }

    /// <summary>
    /// クールタイムのリセット
    /// </summary>
    private void ResetCoolTime()
    {
        D_Colltime = CoolTime;
        MagicCoolTimeText.text = " ";
    }

    #region 各当たり判定処理
     
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
