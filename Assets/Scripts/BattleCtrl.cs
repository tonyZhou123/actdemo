using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCtrl : MonoBehaviour {


    public List<GameObject> pool;

    // Use this for initialization
    void Start () {

        GameObject sFref = (GameObject)Resources.Load("Prefabs/Actor");
        for(int i = 1; i < 2; ++i)
        {
            if(i == 1) // 临时代码, 配置表内容
            {
                sFref.GetComponent<ActorCtrl>().actorPrefeb = (GameObject)Resources.Load("Prefabs/loy");
                sFref.GetComponent<ActorCtrl>().actorType = 1;
            }
            else
            {
                sFref.GetComponent<ActorCtrl>().actorPrefeb = (GameObject)Resources.Load("Prefabs/emi");
                sFref.GetComponent<ActorCtrl>().actorType = 2;
            }
           
            GameObject sObj 
                = Instantiate(sFref, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
            
            pool.Add(sObj);
        }
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
