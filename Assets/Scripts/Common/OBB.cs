using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBB {

    public Vector3[] m_corners;

    public OBB(BoxCollider boxcollider)
    {
        m_corners = new Vector3[8];

        // 0-3: z is positive
        m_corners[0] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);
        m_corners[1] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);
        m_corners[2] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, boxcollider.size.z) * 0.5f);
        m_corners[3] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, boxcollider.size.z) * 0.5f);

        // 4-7: z is negative
        m_corners[4] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        m_corners[5] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        m_corners[6] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, -boxcollider.size.y, -boxcollider.size.z) * 0.5f);
        m_corners[7] = boxcollider.transform.TransformPoint(boxcollider.center + new Vector3(-boxcollider.size.x, boxcollider.size.y, -boxcollider.size.z) * 0.5f);
    }

    //将点投影到坐标轴
    public float projectPoint(Vector3 point, Vector3 axis)
    {
        float dot = Vector3.Dot(axis, point); //点积
        return dot;
    }

    //计算最大最小投影值  
    public void getInterval(OBB box, Vector3 axis, out float min, out float max)
    {
        float value;
        //分别投影八个点，取最大和最小值
        min = max = projectPoint(box.m_corners[0], axis);
        for (int i = 1; i<8; i++)
        {
            value = projectPoint(box.m_corners[i], axis);
            if (value < min)
            {
                min = value;
            }
            if (value > max)
            {
                max = value;
            }
        }
    }

    //取边的矢量
    public Vector3 getEdgeDirection(int index)
    {
        Vector3 tmpLine = Vector3.zero;
        switch (index)
        {
            case 0:// x轴方向
                tmpLine = m_corners[5] - m_corners[6];
                break;
            case 1:// y轴方
                tmpLine = m_corners[7] - m_corners[6];
                break;
            case 2:// z轴方向
                tmpLine = m_corners[1] - m_corners[6];
                break;
            default:
                break;
        }
        return tmpLine.normalized;
    }

    //取面的方向矢量  
    public Vector3 getFaceDirection(int index)
    {
        Vector3 faceDirection, v0, v1;

        switch(index)
        {
            case 0: //前/后计算结果为一个与z轴平行的矢量
                v0 = m_corners[2] - m_corners[1];
                v1 = m_corners[0] - m_corners[1];
                faceDirection = Vector3.Cross(v0, v1);
                break;

            case 1: //左/右计算结果为一个与x轴平行的矢量
                v0 = m_corners[5] - m_corners[2];
                v1 = m_corners[3] - m_corners[2];
                faceDirection = Vector3.Cross(v0, v1);
                break;

            case 2: //上/下计算结果为一个与y轴平行的矢量
                v0 = m_corners[1] - m_corners[2];
                v1 = m_corners[5] - m_corners[2];
                //faceDirection = Vector3.Cross(v0, v1);
                faceDirection = Vector3.Cross(v1, v0);
                break;

            default:
                faceDirection = Vector3.zero;
                break;
        }
        return faceDirection.normalized;
    }  

    //检测两个OBB包围盒是否重合
    public bool isIntersects(OBB box)
    {
        float min1, max1, min2, max2;

        //当前包围盒的三个面方向相当于取包围盒的三个坐标轴为分离轴并计算投影作比较
        for (int i = 0; i<3; i++)
        {
            Vector3 faceDir = getFaceDirection(i);
            getInterval(this, faceDir, out min1, out max1); //计算当前包围盒在某轴上的最大最小投影值
            getInterval(box, faceDir, out min2, out max2); //计算另一个包围盒在某轴上的最大最小投影值
            if (max1 < min2 || max2 < min1)
            {
                return false; //判断分离轴上投影是否重合
            }
        }

        //box包围盒的三个面方向
        for (int i = 0; i<3; i++)
        {
            Vector3 faceDir = box.getFaceDirection(i);
            getInterval(this, faceDir, out min1, out max1);
            getInterval(box, faceDir, out min2, out max2);
            if (max1 < min2 || max2 < min1)
            {
                return false;
            }
        }

        for (int i = 0; i<3; i++)
        {
            for (int j = 0; j<3; j++)
            {
                Vector3 axis = Vector3.Cross(getEdgeDirection(i), box.getEdgeDirection(j)); //边的矢量并做叉积
                getInterval(this, axis, out min1, out max1);
                getInterval(box, axis, out min2, out max2);
                if (max1 < min2 || max2 < min1)
                {
                    return false;
                }
            }
        }

        return true;
    }

}
