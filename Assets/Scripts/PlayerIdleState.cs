using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : CommonFSMState {

    protected PlayerController m_controller;

    public PlayerIdleState(PlayerController controller)
    {
        m_controller = controller;
    }

	public override int GetStateID()
    {
        return (int)PlayerController.EnumPlayerState.Idle;
    }

    public override bool OnEnter(CommonFSMState prevState, object param1, object param2)
    {
        //if (!m_controller.m_anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        {
            m_controller.m_anim.SetBool("idle", true);
            //m_controller.m_anim.SetFloat("attackValue", 0);
            //anim.SetTrigger("changeToIdle");
        }
        return true;
    }

    public override bool OnLeave(CommonFSMState nextState, object param1, object param2)
    {
        m_controller.m_anim.SetBool("idle", false);
        return true;
    }
}
