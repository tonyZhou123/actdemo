using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIJoysticks : MonoBehaviour {

    Vector3 m_initPosition;

    public float m_radius;

    protected bool m_bDragging = false;

    // Use this for initialization
    void Start () {
        /*
        border = GameObject.Find("border").transform;
        initPosition = GetComponentInParent<RectTransform>().position;
        r = Vector3.Distance(transform.position, border.position);
        */
        //m_radius = 40;
        m_radius = 40 * (GameObject.Find("Canvas").gameObject.transform.localScale.x);

        m_initPosition = transform.position;
    }

    // Update is called once per frame
    void Update () {
		
	}

    void FixedUpdate()
    {
        if (m_bDragging)
        {
            onDragging();
        }
    }

    protected void onDragging()
    {  
        //如果鼠标到虚拟键盘原点的位置 < 半径r  
        if (Vector3.Distance(Input.mousePosition, m_initPosition) < m_radius)  
        {  
            //虚拟键跟随鼠标  
            transform.position = Input.mousePosition;  
        }  
        else
        {  
            //计算出鼠标和原点之间的向量  
            Vector3 dir = Input.mousePosition - m_initPosition;
            //这里dir.normalized是向量归一化的意思，实在不理解你可以理解成这就是一个方向，就是原点到鼠标的方向，乘以半径你可以理解成在原点到鼠标的方向上加上半径的距离  
            transform.position = m_initPosition + dir.normalized * m_radius;
            //transform.position = initPosition;
        }
;
        //float angle = Vector3.Angle(transform.position - initPosition, transform.right);
        Vector3 dir1 = transform.position - m_initPosition;
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
        float ratio = Vector3.Distance(transform.position, m_initPosition) / m_radius;
        InputController.Instance.notifyDpadDragging(quadrant, Mathf.Abs(angle), ratio);
    }
    
    public void OnDragBegin()
    {
        m_bDragging = true;
    }

    public void OnDragEnd()
    {
        m_bDragging = false;

        //松开鼠标虚拟摇杆回到原点  
        transform.position = m_initPosition;
        //Debug.Log(transform.position);

        InputController.Instance.notifyDpadReleased();
    }  


}
