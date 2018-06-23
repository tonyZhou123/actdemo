using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : StateBase {

  

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
        if (sActor.m_iRunDir == 0)
        {
            sActor.transform.position -= new Vector3(Time.fixedDeltaTime, 0, 0) * 14; //todo zhou
            sActor.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            sActor.transform.position += new Vector3(Time.fixedDeltaTime, 0, 0) * 14;
            sActor.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        }
    }


    public override bool OnEnter(ActorCtrl sActor)
    {
        /*
        if (prevState != null && prevState.GetStateID() == (int)PlayerController.EnumPlayerState.Attack1)
        {
            return false;
        }
        */

        //m_controller.m_anim.SetFloat("speedValue", 1.1f);
        //m_iRunDir = (int)param1;

        return true;
    }

    public override bool OnLeave(ActorCtrl sActor)
    {
        //m_controller.m_anim.SetFloat("speedValue", 0f);
        return true;
    }
}
