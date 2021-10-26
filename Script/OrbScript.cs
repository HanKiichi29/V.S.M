using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class OrbScript : MonoBehaviour
{
    [SerializeField] private GameObject OrbObj;

    [SerializeField] private GameObject Player;

    [SerializeField] private Text CountText;
    

    private int OrbCount=0;
    private GameObject Cloo;

    private Vector3 Pl;

    private void Start()
    {
        Pl = Player.transform.position;

        ReadOrbCountText();

      /*  Observable.EveryUpdate()
            .Subscribe(_ => GettingOrb()).AddTo(this);*/
        
    }

    void GettingOrb()
    {
        OrbCount += 1;
        ReadOrbCountText();
       
    }

    void ReadOrbCountText()
    {
        CountText.text = "オーブ:" + OrbCount.ToString();
    }


    //オーブのドロップ
    public void OrbDrop(GameObject obj, int DropCount)
    {
        for(int i = 0; i < DropCount; i++)
        {
            Cloo = Instantiate(OrbObj,obj.transform.position, Quaternion.identity);

        }

        
    }

   

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == Player.name)
        {
           
            Cloo.SetActive(false);
            GettingOrb();

        }
    }
}
