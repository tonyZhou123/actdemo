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

	public CmdBase(BattleCmdType eType)
    {
        m_eType = eType;
    }

    public BattleCmdType TypeGet()
    {
        return m_eType;
    }
}
