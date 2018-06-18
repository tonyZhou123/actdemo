using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdMgr : MonoBehaviour {

    private LinkedList<CmdBase> m_lCmds;

    public void CmdPush(CmdBase sCmd)
    {
        m_lCmds.AddFirst(sCmd);
    }

    public CmdBase CmdPop()
    {
        CmdBase sCmd = m_lCmds.Last.Value;
        m_lCmds.RemoveLast();
        return sCmd;
    }

    public bool IsCmdEmpty()
    {
        return m_lCmds.Count == 0;
    }


    void LaunchCmd()
    {
        int iCount = 0;
        while(true)
        {
            if(IsCmdEmpty())
            {
                break;
            }

            CmdBase sCmd = CmdPop();
            switch(sCmd.TypeGet())
            {
                case BattleCmdType.BCT_MOVE:
                    {
                        InputController.Instance.notifyDpadDragging(
                            ((CmdMove)sCmd).QuaduantGet()
                            , ((CmdMove)sCmd).AngleGet()
                            , ((CmdMove)sCmd).RatioGet());
                    }
                    break;

                case BattleCmdType.BCT_ATTACK:
                    {
                        InputController.Instance.notifyBtnAttackClicked();
                    }
                    break;
                default:
                    break;
                
            }

            ++iCount;
            if(iCount > 100) //每次最多处理100条
            {
                Debug.LogError("LaunchCmd over 100");
                break;
            }
        }
    }
    public void Awake()
    {
        InvokeRepeating("LaunchCmd", 0.1f, 0.1f);  //0.1秒后，每0.1f调用一次
    }



    // Use this for initialization
    void Start () {
        m_lCmds = new LinkedList<CmdBase>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
