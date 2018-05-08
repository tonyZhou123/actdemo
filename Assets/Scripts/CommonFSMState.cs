
public abstract class CommonFSMState
{
    public abstract int GetStateID();

    public virtual bool OnLeave(CommonFSMState nextState, object param1, object param2)
    {
        return true;
    }

    public virtual bool OnEnter(CommonFSMState prevState, object param1, object param2)
    {
        return true;
    }

    public virtual void BreakInto(CommonFSMState nextState, object param1, object param2)
    {
    }
}