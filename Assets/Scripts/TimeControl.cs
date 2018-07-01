using System.Collections.Generic;
using Framework;
using UnityEngine;

public class TimeControl : Singleton<TimeControl>
{
    public List<object> Objects = new List<object>();
    private int Count = 0;

    public void Set(object obj)
    {
        Objects.Add(obj);
        SetTime();
    }

    public void Remove(object obj)
    {
        Objects.Remove(obj);
        SetTime();
    }

    private void SetTime()
    {
        if (Objects.Count > 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
