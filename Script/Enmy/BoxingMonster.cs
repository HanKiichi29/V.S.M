using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �G�L����I am�X�g�[�J�[is�i���o�[ONE!�̏���
/// </summary>
public class BoxingMonster : AllEnmy
{

    private GameObject player;

    private BoxCollider BoxingCollider;

    private void Start()
    {
        player = GameObject.Find("Player");

        EnmyAnimation = GetComponent<Animator>();
        BoxingCollider = GetComponent<BoxCollider>();

    }

    protected override void EnmyCameraUpdate()
    {
        base.EnmyCameraUpdate();

        if (Stop)
        {
           
            if (Rendered)
            {
               
                //�v���C���[���W�����v�������ɏ�ɒǂ�Ȃ�����
                if (transform.position.y < 1.5f)
                {
                    float BoxingMonsterSpeed = EnmySpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, player.transform.position, BoxingMonsterSpeed);
                }
              
                var direction = player.transform.position - transform.position;
                direction.y = 0;
                var rotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);

            }
        }

  
    }

    protected override void DeadEnmy()
    {
        base.DeadEnmy();

        BoxingCollider.isTrigger = true;

    }

}
