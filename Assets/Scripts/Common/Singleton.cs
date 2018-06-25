using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework
{
    public class SingletonBase
    {
        protected static List<SingletonBase> s_lstSingle = new List<SingletonBase>();
        public static void DestroyAll()
        {
            for (int i = s_lstSingle.Count - 1; i >= 0; --i)
            {
                SingletonBase s = s_lstSingle[i];
                s.Destroy();
            }
            if (s_lstSingle.Count != 0)
            {
                Debug.LogError("s_lst.Count != 0");
            }
            s_lstSingle.Clear();
        }

        virtual public void Init()
        {

        }

        virtual protected void OnDestroy()
        {

        }

        virtual protected void Tick()
        {

        }

        virtual public void Destroy()
        {
        }

    }

    public abstract class Singleton<T>:SingletonBase where T : SingletonBase, new()
    {
        private static T _instance = null;
        public static T instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                    _instance.Init();
                    s_lstSingle.Add(_instance);
                    Debug.Log("+++++++++++++++ " + _instance.GetType().ToString());
                }
                return _instance;
            }
        }

        Tween _t = null;
        protected void TriggerTick()
        {
            _t = Tween.Tick(null, Tick);
        }

        override public void Destroy()
        {
            Tween.Stop(ref _t);

            OnDestroy();
            s_lstSingle.Remove(_instance);
            Debug.Log("--------------- " + _instance.GetType().ToString());
            _instance = null;
        }
    }
}