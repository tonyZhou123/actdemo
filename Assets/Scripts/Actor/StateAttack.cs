using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAttack : StateBase {

	public override BattleActorStateType GetStateID()
    {
        return BattleActorStateType.BAST_ATTACK;
    }

    public override void InputHandle(ActorCtrl sActor, BattleInputType param)
    {
        switch (param)
        {
            case BattleInputType.BIT_ATTACK:
                {

                }
                break;
            case BattleInputType.BIT_MOVE:
                {

                }
                break;
            default:
                break;
        }
    }

    public override void Update(ActorCtrl sActor)
    {

    }

    public override bool OnEnter(StateBase prevState, object param1, object param2)
    {
        //m_controller.m_anim.SetFloat("attackValue", 1.0f);
        return true;
    }

    public override bool OnLeave(StateBase nextState, object param1, object param2)
    {
        //int nextStateID = nextState.GetStateID();
        //
        //if (nextStateID == (int)PlayerController.EnumPlayerState.Attack1)
        //{
        //    return false;
        //}
        //else
        //if (nextStateID == (int)PlayerController.EnumPlayerState.Run)
        //{
        //    if (m_controller.m_anim.IsInTransition(0))
        //    {
        //        return false;
        //    }
        //    AnimatorStateInfo animatorInfo = m_controller.m_anim.GetCurrentAnimatorStateInfo(0);
        //    if (animatorInfo.normalizedTime <= 1.0f) // play not completed
        //    {
        //        return false;
        //    }
        //}
        //
        //m_controller.m_anim.SetFloat("attackValue", 0);
        return true;
    }
}
