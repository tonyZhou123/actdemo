using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleCmdType
{
    BCT_NULL,
    BCT_MOVE,
    BCT_ATTACK,
}


public class CmdBase {

    protected BattleCmdType m_eType;
    protected int m_iIdx;

	public CmdBase(BattleCmdType eType,int iIdx)
    {
        m_eType = eType;
        m_iIdx = iIdx;
    }

    public BattleCmdType TypeGet()
    {
        return m_eType;
    }

    public int IdxGet()
    {
        return m_iIdx;
    }
}
