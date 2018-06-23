using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdAttack : CmdBase
{
    public CmdAttack(int iIdx)
     : base(BattleCmdType.BCT_ATTACK,iIdx)
    {
  
    }
}
