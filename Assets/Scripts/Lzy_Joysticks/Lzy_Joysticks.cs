using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Lzy_Joysticks : MonoBehaviour
{
    private Transform m_joystickBg;
    private Transform m_joystickBtn;
    private CanvasGroup canvasGroup;

    protected bool m_bDragging = false;
    private Vector3 m_originJoystickBgPos;
    private Vector3 m_originJoystickBtnPos;
    private Vector3 m_initPosition;
    private float m_radius;
    private CmdMgr m_sCmdMgr;

    void Awake()
    {
        m_joystickBg = this.transform.Find("JostickBg");
        m_joystickBtn = m_joystickBg.Find("JostickBtn");
        canvasGroup = m_joystickBg.GetComponent<CanvasGroup>();
        m_radius = m_joystickBg.GetComponent<RectTransform>().sizeDelta.x -
                   m_joystickBtn.GetComponent<RectTransform>().sizeDelta.x;
        m_radius *= GameObject.Find("Canvas").gameObject.transform.localScale.x;
        m_originJoystickBgPos = m_joystickBg.localPosition;
        m_originJoystickBtnPos = m_joystickBtn.localPosition;
        //SetJoysticksAcitve(false);
        SetJoysticksAlpha(0.3f);
        AddListener();
        m_sCmdMgr = GameObject.Find("Cube").GetComponent<CmdMgr>();
    }

    //不设置Active
    //void SetJoysticksAcitve(bool flag)
    //{
    //    m_joystickBg.gameObject.SetActive(flag);
    //    m_joystickBtn.gameObject.SetActive(flag);
    //}
    //设置Alpha
    void SetJoysticksAlpha(float value)
    {
        canvasGroup.alpha = value;
    }

    void AddListener()
    {
        UIEventListener.Get(this.gameObject).onDown = ClickDown;
        UIEventListener.Get(this.gameObject).onDragBegin = BeginDrag;
        UIEventListener.Get(this.gameObject).onDrag = Draging;
        UIEventListener.Get(this.gameObject).onDragEnd = EndDrag;
        UIEventListener.Get(this.gameObject).onUp = ClickUp;
    }

    //void FixedUpdate()
    //{
    //    if (m_bDragging)
    //    {
    //        Draging(null,null);
    //    }
    //}

    void ClickDown(GameObject go)
    {
        //SetJoysticksAcitve(true);
        SetJoysticksAlpha(1f);
        m_joystickBg.position = Input.mousePosition;
        m_joystickBtn.position = Input.mousePosition;
        m_initPosition = m_joystickBg.position;
    }

    void BeginDrag(GameObject go)
    {
        //m_bDragging = true;
    }

    void Draging(GameObject go, PointerEventData data)
    {
        //如果鼠标到虚拟键盘原点的位置 < 半径r  
        if (Vector3.Distance(Input.mousePosition, m_initPosition) < m_radius)
        {
            //虚拟键跟随鼠标  
            m_joystickBtn.position = Input.mousePosition;
        }
        else
        {
            //计算出鼠标和原点之间的向量  
            Vector3 dir = Input.mousePosition - m_initPosition;
            //这里dir.normalized是向量归一化的意思，实在不理解你可以理解成这就是一个方向，就是原点到鼠标的方向，乘以半径你可以理解成在原点到鼠标的方向上加上半径的距离  
            m_joystickBtn.position = m_initPosition + dir.normalized * m_radius;
        }
;
        //float angle = Vector3.Angle(transform.position - initPosition, transform.right);
        Vector3 dir1 = m_joystickBtn.position - m_initPosition;
        int quadrant = 0;
        if (dir1.x >= 0)
        {
            if (dir1.y >= 0)
            {
                quadrant = 1;
            }
            else
            {
                quadrant = 4;
            }
        }
        else
        {
            if (dir1.y >= 0)
            {
                quadrant = 2;
            }
            else
            {
                quadrant = 3;
            }
        }
        float angle = Mathf.Atan(dir1.y / dir1.x) * 180 / Mathf.PI;
        //Debug.Log("x: " + dir1.x + "; y: " + dir1.y);
        float ratio = Vector3.Distance(m_joystickBtn.position, m_initPosition) / m_radius;

        CmdMove sCmd = new CmdMove(1);
        sCmd.Init(quadrant, Mathf.Abs(angle), ratio);

        m_sCmdMgr.CmdPush((CmdBase)sCmd);

        //InputController.Instance.notifyDpadDragging(quadrant, Mathf.Abs(angle), ratio);
    }

    void EndDrag(GameObject go)
    {
        //m_bDragging = false;

        InputController.Instance.notifyDpadReleased();
    }

    void ClickUp(GameObject go)
    {
        //松开鼠标虚拟摇杆回到原点
        m_joystickBg.localPosition = m_originJoystickBgPos;
        m_joystickBtn.localPosition = m_originJoystickBtnPos;
        //SetJoysticksAcitve(false);
        SetJoysticksAlpha(0.3f);
    }

        
}
