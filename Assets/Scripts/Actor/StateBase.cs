﻿

public enum BattleInputType
{ 
    BIT_MOVE,
    BIT_ATTACK,
    BIT_IDEL,
}


public enum BattleActorStateType
{
    BAST_IDLE,
    BAST_RUN,
    BAST_ATTACK,
}

public abstract class StateBase
{
    public abstract BattleActorStateType GetStateID();

    public virtual void InputHandle(ActorCtrl sActor, BattleInputType param)
    {

    }

    public virtual void Update(ActorCtrl sActor)
    {

    }

    public virtual bool OnLeave(StateBase nextState, object param1, object param2)
    {
        return true;
    }

    public virtual bool OnEnter(StateBase prevState, object param1, object param2)
    {
        return true;
    }

    public virtual void BreakInto(StateBase nextState, object param1, object param2)
    {
    }
}