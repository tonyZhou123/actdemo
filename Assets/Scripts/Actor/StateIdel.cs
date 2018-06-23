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
                    sActor.StateSet(new StateRun());
                }
                break;
            default:
                break;
        }
    }
    // string     animString = anim.GetCurrentAnimationClipState(0)[0].clip.name;

    public override void Update(ActorCtrl sActor)
    {

   

    }


    public override bool OnEnter(ActorCtrl sActor)
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



        Animator m_anim = sActor.actorObj.GetComponent<Animator>();
        AnimatorStateInfo animatorInfo = m_anim.GetCurrentAnimatorStateInfo(0);
        //m_anim.Play("run",0,0);
        m_anim.CrossFade("idle", 0.2f, 0);
        if (animatorInfo.normalizedTime >= 1.0f)
        {
            Debug.Log("Update" + animatorInfo.normalizedTime);
        }




        return true;
    }

    public override bool OnLeave(ActorCtrl sActor)
    {
        //m_controller.m_anim.SetBool("idle", false);
        return true;
    }
}
