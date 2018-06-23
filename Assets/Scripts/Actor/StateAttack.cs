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

    public override bool OnEnter(ActorCtrl sActor)
    {
        //m_controller.m_anim.SetFloat("attackValue", 1.0f);
        return true;
    }

    public override bool OnLeave(ActorCtrl sActor)
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
