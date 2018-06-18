using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public enum EnumPlayerState
    {
        Idle,
        Run,
        Attack1,
        Attack2,
        Attack3
    };

    public float m_speed = 2f;
    public Animator m_anim;

    public CommonFSM m_fsm;

    void Awake()
    {
        m_anim = this.GetComponent<Animator>();

        m_fsm = new CommonFSM();
        m_fsm.AddState(new PlayerIdleState(this), true);
        m_fsm.AddState(new PlayerRunState(this));
        m_fsm.AddState(new PlayerAttack1State(this));

        InputController.Instance.onDpadDraggingEvent += onDpadDragging;
        InputController.Instance.onDpadReleasedEvent += onDpadReleased;
        InputController.Instance.onBtnAttackClickedEvent += onBtnAttackClicked;
    }

    // Use this for initialization
    void Start () {
        m_fsm.SwitchState((int)EnumPlayerState.Idle);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    void FixedUpdate()
    {
        CommonFSMState curState = m_fsm.GetCurState();
        if (curState != null && curState.GetStateID() == (int)EnumPlayerState.Run)
        {
            if (((PlayerRunState)curState).m_iRunDir == 0)
            {
                this.transform.position -= new Vector3(Time.fixedDeltaTime, 0, 0) * m_speed;
                this.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
            else
            {
                this.transform.position += new Vector3(Time.fixedDeltaTime, 0, 0) * m_speed;
                this.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
            }
        }

        ChangeToIdle();
    }

    void ChangeToIdle()
    {
        AnimatorStateInfo animatorInfo = m_anim.GetCurrentAnimatorStateInfo(0);
        if (animatorInfo.normalizedTime > 1.0f && animatorInfo.IsName("Base Layer.attack1"))
        {
            m_fsm.SwitchState((int)EnumPlayerState.Idle);
        }
    }


    public void onDpadDragging(int quadrant, float angle, float ratio)
    {
        Debug.Log("quadrant: " + quadrant + " angle: " + angle + " ratio: " + ratio);


        if (quadrant == 1 || quadrant == 4)
        {
            m_fsm.SwitchState((int)EnumPlayerState.Run, 1);
        }
        else
        {
            m_fsm.SwitchState((int)EnumPlayerState.Run, 0);
        }
    }

    public void onDpadReleased()
    {
        m_fsm.SwitchState((int)EnumPlayerState.Idle);
    }

    public void onBtnAttackClicked()
    {
        m_fsm.SwitchState((int)EnumPlayerState.Attack1);
    }
}
