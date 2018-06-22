using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdel : StateBase {

    public override BattleActorStateType GetStateID()
    {
        return BattleActorStateType.BAST_IDLE;
    }

    public int temp = 1;

    public override void InputHandle(ActorCtrl sActor, BattleInputType param)
    {
        switch(param)
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
    // string     animString = anim.GetCurrentAnimationClipState(0)[0].clip.name;

    public override void Update(ActorCtrl sActor)
    {

        Animator m_anim = sActor.actorObj.GetComponent<Animator>();
        AnimatorStateInfo animatorInfo = m_anim.GetCurrentAnimatorStateInfo(0);

       
        if (animatorInfo.normalizedTime >= 1.0f)
        {
            m_anim.CrossFade("run", 0.2f,0);
            Debug.Log("Update" + animatorInfo.normalizedTime);
        }


    }


    public override bool OnEnter(StateBase prevState, object param1, object param2)
    {
        //if (!m_controller.m_anim.GetCurrentAnimatorStateInfo(0).IsName("idle"))
        //{
        //    m_controller.m_anim.SetBool("idle", true);
        //    //m_controller.m_anim.SetFloat("attackValue", 0);
        //    //anim.SetTrigger("changeToIdle");
        //}

        //Animator m_anim = sActor.actorObj.GetComponent<Animator>();
        //
        //m_anim.CrossFade("idle", 0.2f, 0);

        return true;
    }

    public override bool OnLeave(StateBase nextState, object param1, object param2)
    {
        //m_controller.m_anim.SetBool("idle", false);
        return true;
    }
}
