using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdMove : CmdBase {

    private int m_iQuadrant;
    private float m_fAngle;
    private float m_fRatio;

    public CmdMove(int iIdx)
        :base(BattleCmdType.BCT_MOVE,iIdx)
    {
        m_iQuadrant = 0;
        m_fAngle = 0;
        m_fRatio = 0;
    }

    public void Init(int iQuadrant,float fAngle,float fRatio)
    {
        m_iQuadrant = iQuadrant;
        m_fAngle = fAngle;
        m_fRatio = fRatio;
    }

    public int QuaduantGet()
    {
        return m_iQuadrant;
    }

    public float AngleGet()
    {
        return m_fAngle;
    }

    public float RatioGet()
    {
        return m_fRatio;
    }
}
