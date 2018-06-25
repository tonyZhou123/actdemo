using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void EventCallback(params object [] args);

public class EventData
{
    public EventCallback callback;
    public object        target;
}

public class EventCenter
{
    static Dictionary<int, List<EventData>> s_Callbacks       = new Dictionary<int, List<EventData>>();
    static List<int>                        s_TargetEventList = new List<int>(8);

    public static void Destroy()
    {
        s_Callbacks.Clear();
    }

    public static void AddListener(int eventID, EventCallback callback, object target)
    {
        if (callback == null)
        {
            return;
        }
        List<EventData> list = null;
        s_Callbacks.TryGetValue(eventID,out list);
        if (list == null)
        {
            list = new List<EventData>();
            s_Callbacks.Add(eventID, list);
        }
        bool isExist = false;
        for (int i = 0; i < list.Count; i++)
        {
            EventData data = list[i];
            if (data.callback == callback)
            {
                isExist = true;
            }
        }
        if (isExist == false)
        {
            EventData data = new EventData();
            data.callback = callback;
            data.target = target;
            list.Add(data);
        }
    }

    public static void DelListener(int eventID, EventCallback callback)
    {
        if (callback == null)
        {
            return;
        }
        List<EventData> eventList = null;
        s_Callbacks.TryGetValue(eventID, out eventList);
        if (eventList == null)
        {
            return;
        }
        for (int i = eventList.Count - 1; i >= 0; i--)
        {
            if(eventList[i].callback == callback)
            {
                eventList.RemoveAt(i);
            }
        }
        if (eventList.Count == 0)
        {
            s_Callbacks.Remove(eventID);
        }
    }

    public static void AddListener(int eventID, EventCallback callback)
    {
        AddListener(eventID, callback, null);
    }



    public static void DelListenersByTarget(object target)
    {
        if (target == null)
        {
            return;
        }
        s_TargetEventList.Clear();
        foreach (var current in s_Callbacks)
        {
            s_TargetEventList.Add(current.Key);
        }
        for (int i = 0; i < s_TargetEventList.Count; i++)
        {
            int eventID = s_TargetEventList[i];
            List<EventData> eventList = null;
            s_Callbacks.TryGetValue(eventID, out eventList);
            if (eventList != null)
            {
                for (int k = eventList.Count - 1; k >= 0; k--)
                {
                    if(eventList[k].target == target)
                    {
                        eventList.RemoveAt(k);
                    }
                }
                if (eventList.Count == 0)
                {
                    s_Callbacks.Remove(eventID);
                }
            }
        }
        s_TargetEventList.Clear();
    }

    public static void Notify(int eventID, params object[] args)
    {
        List<EventData> list = null;
        s_Callbacks.TryGetValue(eventID, out list);
        if (list == null)
        {
            return;
        }
        for (int i = 0; i < list.Count; i++)
        {
            EventData data = list[i];
            if (data.callback != null)
            {
                data.callback(args);
            }
        }
    }

    public static void Notify(int eventID)
    {
        Notify(eventID, string.Empty);
    }
}
