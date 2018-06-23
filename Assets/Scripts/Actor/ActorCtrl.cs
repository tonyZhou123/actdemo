using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCtrl : MonoBehaviour {

    public GameObject actorPrefeb;   
    public GameObject actorObj;   //actor 实体 
    public int actorType;
    public float m_speed = 2f;
    public Animator m_anim;
    private StateBase m_sCurState;
    public int m_iRunDir; // 0: left; 1: right


    // 动画名 ， 应该配表， 临时存这里





    void StateSet(StateBase sState)
    {
        m_sCurState = sState;
        m_sCurState.OnEnter(this);
    }

    // Use this for initialization
    void Start () {
        actorObj = Instantiate(actorPrefeb, GetComponent<Transform>().position, GetComponent<Transform>().rotation);
        StateSet(new StateIdel());
        m_iRunDir = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        m_anim = this.GetComponent<Animator>();
        
        InputController.Instance.onDpadDraggingEvent += onDpadDragging;
        InputController.Instance.onDpadReleasedEvent += onDpadReleased;
        InputController.Instance.onBtnAttackClickedEvent += onBtnAttackClicked;
    }


    void FixedUpdate()
    {
        m_sCurState.Update(this);


        //CommonFSMState curState = m_fsm.GetCurState();
        //if (curState != null && curState.GetStateID() == (int)EnumPlayerState.Run)
        //{
        //    if (((PlayerRunState)curState).m_iRunDir == 0)
        //    {
        //        this.transform.position -= new Vector3(Time.fixedDeltaTime, 0, 0) * m_speed;
        //        this.transform.rotation = Quaternion.Euler(Vector3.zero);
        //    }
        //    else
        //    {
        //        this.transform.position += new Vector3(Time.fixedDeltaTime, 0, 0) * m_speed;
        //        this.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        //    }
        //}
        //
        //ChangeToIdle();
    }

    //void ChangeToIdle()
    //{
    //    AnimatorStateInfo animatorInfo = m_anim.GetCurrentAnimatorStateInfo(0);
    //    if (animatorInfo.normalizedTime > 1.0f && animatorInfo.IsName("Base Layer.attack1"))
    //    {
    //        m_fsm.SwitchState((int)EnumPlayerState.Idle);
    //    }
    //}


    public void onDpadDragging(int quadrant, float angle, float ratio)
    {
        //Debug.Log("quadrant: " + quadrant + " angle: " + angle + " ratio: " + ratio);
        if (quadrant == 1 || quadrant == 4)
        {
            m_iRunDir = 1;
        }
        else
        {
            m_iRunDir = 0;
        }
        m_sCurState.InputHandle(this, BattleInputType.BIT_MOVE);
    }

    public void onDpadReleased()
    {
        m_sCurState.InputHandle(this, BattleInputType.BIT_IDEL);
    }

    public void onBtnAttackClicked()
    {
        m_sCurState.InputHandle(this, BattleInputType.BIT_ATTACK);
    }





}
