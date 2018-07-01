using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorData
{
    //一些必要的配置参数
    public int actorId;//每个Actor的唯一id
    public int configId;//代表是使用哪个，用来实例化不同的角色模型,表格的id
    public string actorPath;//角色的路径，有了configId，后期就直接读表
    public List<int> weapons;//角色的武器id，后期通过读表获取路径
    public bool playerControl;//是否是角色摇杆控制的

    //具体的一些参数（进入战斗场景时需要的参数）
    public int curHp = -1;//如果curHp大于等于0，初始化时就使用curHp，否则默认是满血
    public int maxHp;//角色的血量最大值

    public List<int> skills;//角色技能
    //这里有些是可以后期配表配置
    public float runspeed;//角色移动速度
    public float HurtTime;//角色的受击速度

}
