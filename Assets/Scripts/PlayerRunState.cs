using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : CommonFSMState {

    protected PlayerController m_controller;

    public int m_iRunDir; // 0: left; 1: right

    public PlayerRunState(PlayerController controller)
    {
        m_controller = controller;
    }

	public override int GetStateID()
    {
        return (int)PlayerController.EnumPlayerState.Run;
    }


    public override bool OnEnter(CommonFSMState prevState, object param1, object param2)
    {
        /*
        if (prevState != null && prevState.GetStateID() == (int)PlayerController.EnumPlayerState.Attack1)
        {
            return false;
        }
        */

        m_controller.m_anim.SetFloat("speedValue", 1.1f);
        m_iRunDir = (int)param1;

        return true;
    }

    public override bool OnLeave(CommonFSMState nextState, object param1, object param2)
    {
        m_controller.m_anim.SetFloat("speedValue", 0f);
        return true;
    }
}
