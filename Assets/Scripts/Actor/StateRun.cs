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
            case BattleInputType.BIT_REST:
                {
                    sActor.StateSet(new StateIdel());
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
            sActor.actorObj.transform.position -= new Vector3(Time.fixedDeltaTime, 0, 0) * 14; //todo zhou
            sActor.actorObj.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            sActor.actorObj.transform.position += new Vector3(Time.fixedDeltaTime, 0, 0) * 14;
            sActor.actorObj.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        }

    }


    public override bool OnEnter(ActorCtrl sActor)
    {
        sActor.actorObj.GetComponent<Animator>().CrossFade("run", 0.1f, 0);
        return true;
    }

    public override bool OnLeave(ActorCtrl sActor)
    {
        return true;
    }
}
