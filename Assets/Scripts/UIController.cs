using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public static UIController Instance = null;
    private CmdMgr m_sCmdMgr;
  

    void Awake()
    {
        Instance = this;
        GameObject btnAttack = GameObject.Find("Canvas/BtnAttack");
        Button btn1 = (Button)btnAttack.GetComponent<Button>();
        btn1.onClick.AddListener(delegate () {
            OnBtnAttackClicked();
        });

        m_sCmdMgr = GameObject.Find("Cube").GetComponent<CmdMgr>();
    }

    void OnBtnAttackClicked()
    {
        //InputController.Instance.notifyBtnAttackClicked(btnAttack);
        CmdBase sCmd = new CmdAttack();
        //sCmd.Init(btnAttack);

        m_sCmdMgr.CmdPush((CmdBase)sCmd);
    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
