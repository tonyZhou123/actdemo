using System.Collections;
using System.Collections.Generic;

public class CommonFSM {

    protected Dictionary<int, CommonFSMState> m_dictState;
    protected CommonFSMState m_curState;
    protected CommonFSMState m_defaultState;

    public CommonFSM()
    {
        m_curState = null;
        m_dictState = new Dictionary<int, CommonFSMState>();
    }

    public bool AddState(CommonFSMState state, bool bDefault = false)
    {
        if (state == null)
        {
            return false;
        }

        if (m_dictState.ContainsKey(state.GetStateID()))
        {
            return false;
        }

        m_dictState[state.GetStateID()] = state;

        if (bDefault)
        {
            m_defaultState = state;
        }

        return true;
    }

    // to do: RemoveState()

    public CommonFSMState GetState(int iStateID)
    {
        CommonFSMState ret = null;
        m_dictState.TryGetValue(iStateID, out ret);
        return ret;
    }

    public CommonFSMState GetCurState()
    {
        return m_curState;
    }

    public int GetCurStateID()
    {
        CommonFSMState state = GetCurState();
        return state.GetStateID();
    }

    public bool SwitchState(int iNewStateID, object param1 = null, object param2 = null)
    {
        CommonFSMState newState = null;
        m_dictState.TryGetValue(iNewStateID, out newState);
        if (newState == null)
        {
            return false;
        }

        CommonFSMState oldState = m_curState;

        bool bRet = true;
        if (oldState != null)
        {
            bRet = oldState.OnLeave(newState, param1, param2);
        }
        if (!bRet)
        {
            return false;
        }


        bRet = newState.OnEnter(oldState, param1, param2);
        if (!bRet)
        {
            newState = m_defaultState;
            m_defaultState.BreakInto(oldState, param1, param2);
        }
        m_curState = newState;

        return true;
    }
}
