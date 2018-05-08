using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack1State : CommonFSMState {

    protected PlayerController m_controller;

    public PlayerAttack1State(PlayerController controller)
    {
        m_controller = controller;
    }

	public override int GetStateID()
    {
        return (int)PlayerController.EnumPlayerState.Attack1;
    }

    public override bool OnEnter(CommonFSMState prevState, object param1, object param2)
    {
        m_controller.m_anim.SetFloat("attackValue", 1.0f);
        return true;
    }

    public override bool OnLeave(CommonFSMState nextState, object param1, object param2)
    {
        int nextStateID = nextState.GetStateID();

        if (nextStateID == (int)PlayerController.EnumPlayerState.Attack1)
        {
            return false;
        }
        else
        if (nextStateID == (int)PlayerController.EnumPlayerState.Run)
        {
            if (m_controller.m_anim.IsInTransition(0))
            {
                return false;
            }
            AnimatorStateInfo animatorInfo = m_controller.m_anim.GetCurrentAnimatorStateInfo(0);
            if (animatorInfo.normalizedTime <= 1.0f) // play not completed
            {
                return false;
            }
        }

        m_controller.m_anim.SetFloat("attackValue", 0);
        return true;
    }
}
