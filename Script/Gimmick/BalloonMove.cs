using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

/// <summary>
/// ステージ内の気球
/// </summary>
public class BalloonMove : MonoBehaviour
{

    [SerializeField] private Transform WoopPos;

    [SerializeField] private GameObject Balloon;
    [SerializeField] private GameObject GetOffObject;

     private PlayerController controller;
     private GameObject player;

    private float Balloonforeground = 20;
    private float BalloonBack = 60;

    Vector3 StartPosition;

    private void Start()
    {
        var P= GameObject.FindGameObjectWithTag("Player");
        player = P;
        controller = P.GetComponent<PlayerController>();

        StartPosition = Balloon.transform.position;
    }


    #region 気球に乗る


    //気球に近づいたとき
    private void OnTriggerStay(Collider other)
    {
        if (other.name == player.name)
        {

            player.transform.position = WoopPos.transform.position;
            controller.Move = false;

            if (Balloon.transform.position.z == BalloonBack)
            {
                BalloonMovePlay(Balloonforeground);
            }
            else
            {
                BalloonMovePlay(BalloonBack);
            }

        }
    }

    void BalloonMovePlay(float MoveEndPos)
    {
        //二秒待って移動
        Balloon.transform.DOMoveZ(MoveEndPos, 1.5f)
        .SetDelay(2)
        .SetLink(gameObject)
        .OnComplete(() => { GetOff(); });

    }

    void GetOff()
    {
        var GoalPos = GetOffObject.transform.position + Vector3.right * 2;
        player.transform.position = GoalPos;

        Invoke("ResetBalloon", 1.5f);

    }

    void ResetBalloon()
    {
        Balloon.transform.position = StartPosition;
    }

    #endregion

}
